using BookSleeve;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace common
{
    public class Database : RedisConnection
    {
        private static ILog log = LogManager.GetLogger(nameof(Database));

        public Database(string host, int port, string password)
            : base(host, port, password: password == "" ? null : password)
        {
            SetKeepAlive(60);
            Open().Wait();
        }

        private static string[] defaultNames = new string[] {
            "Darq", "Deyst", "Drac", "Drol",
            "Eango", "Eashy", "Eati", "Eendi", "Ehoni",
            "Gharr", "Iatho", "Iawa", "Idrae", "Iri", "Issz", "Itani",
            "Laen", "Lauk", "Lorz",
            "Oalei", "Odaru", "Oeti", "Orothi", "Oshyu",
            "Queq", "Radph", "Rayr", "Ril", "Rilr", "Risrr",
            "Saylt", "Scheev", "Sek", "Serl", "Seus",
            "Tal", "Tiar", "Uoro", "Urake", "Utanu",
            "Vorck", "Vorv", "Yangu", "Yimi", "Zhiar"
        };

        public DbAccount CreateGuestAccount(string uuid)
        {
            return new DbAccount(this, "0")
            {
                UUID = uuid,
                Name = defaultNames[(uint)uuid.GetHashCode() % defaultNames.Length],
                Admin = false,
                NameChosen = false,
                Verified = false,
                Converted = false,
                GuildId = "-1",
                GuildRank = -1,
                VaultCount = 1,
                MaxCharSlot = 1,
                RegTime = DateTime.Now,
                Guest = true,
                Fame = 0,
                TotalFame = 0,
                Credits = 1000,
                FortuneTokens = 0,
                Gifts = new int[] { 0xae9 },
                PetYardType = 0,
                IsAgeVerified = 0,
                Rank = 0,
            };
        }

        public LoginStatus Verify(string uuid, string password, out DbAccount acc)
        {
            acc = null;
            var info = new DbLoginInfo(this, uuid);
            if (info.IsNull)
                return LoginStatus.AccountNotExists;

            var userPass = Utils.SHA1(password + info.Salt);
            if (Convert.ToBase64String(userPass) != info.HashedPassword)
                return LoginStatus.InvalidCredentials;

            acc = new DbAccount(this, info.AccountId);

            return LoginStatus.OK;
        }

        public bool AcquireLock(DbAccount acc)
        {
            string lockToken = Guid.NewGuid().ToString();
            string key = "lock." + acc.AccountId;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyNotExists(1, key));

                trans.Strings.Set(1, key, lockToken);
                trans.Keys.Expire(1, key, 60);

                bool ok = trans.Execute().Exec();
                acc.LockToken = ok ? lockToken : null;
                return ok;
            }
        }

        public int GetLockTime(DbAccount acc) => (int)Keys.TimeToLive(1, $"lock.{acc.AccountId}").Exec();

        public int GetLockTime(string accId) => (int)Keys.TimeToLive(1, $"lock.{accId}").Exec();

        public bool RenewLock(DbAccount acc)
        {
            string key = $"lock.{acc.AccountId}";
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, acc.LockToken));
                Keys.Expire(1, key, 60);
                return trans.Execute().Exec();
            }
        }

        public void ReleaseLock(DbAccount acc)
        {
            string key = $"lock.{acc.AccountId}";
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, acc.LockToken));
                trans.Keys.Remove(1, key);
                trans.Execute().Exec();
            }
        }

        public IDisposable Lock(DbAccount acc) => new l(this, acc);

        public bool LockOk(IDisposable l) => ((l)l).lockOk;

        private struct l : IDisposable
        {
            private Database db;
            private DbAccount acc;
            internal bool lockOk;

            public l(Database db, DbAccount acc)
            {
                this.db = db;
                this.acc = acc;
                lockOk = db.AcquireLock(acc);
            }

            public void Dispose()
            {
                if (lockOk)
                    db.ReleaseLock(acc);
            }
        }

        public const string REG_LOCK = "regLock";
        public const string NAME_LOCK = "nameLock";

        public string AcquireLock(string key)
        {
            string lockToken = Guid.NewGuid().ToString();
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyNotExists(1, key));

                trans.Strings.Set(1, key, lockToken);
                trans.Keys.Expire(1, key, 60);

                return trans.Execute().Exec() ? lockToken : null;
            }
        }

        public void ReleaseLock(string key, string token)
        {
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, key, token));
                trans.Keys.Remove(1, key);
                trans.Execute();
            }
        }

        public bool RenameUUID(DbAccount acc, string newUuid, string lockToken)
        {
            string p = Hashes.GetString(0, "login", acc.UUID.ToUpperInvariant()).Exec();
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, REG_LOCK, lockToken));
                trans.Hashes.Remove(0, "login", acc.UUID.ToUpperInvariant());
                trans.Hashes.Set(0, "login", newUuid.ToUpperInvariant(), p);
                if (!trans.Execute().Exec()) return false;
            }
            acc.UUID = newUuid;
            acc.Flush();
            return true;
        }

        public bool RenameIGN(DbAccount acc, string newName, string lockToken)
        {
            if (defaultNames.Contains(newName, StringComparer.InvariantCultureIgnoreCase))
                return false;
            using (var trans = CreateTransaction())
            {
                trans.AddCondition(Condition.KeyEquals(1, NAME_LOCK, lockToken));
                Hashes.Remove(0, "names", acc.Name.ToUpperInvariant());
                Hashes.Set(0, "names", newName.ToUpperInvariant(), acc.AccountId.ToString());
                if (!trans.Execute().Exec()) return false;
            }
            acc.Name = newName;
            acc.NameChosen = true;
            acc.Flush();
            return true;
        }

        private static RandomNumberGenerator gen = RandomNumberGenerator.Create();

        public void ChangePassword(string uuid, string password)
        {
            var login = new DbLoginInfo(this, uuid);

            var x = new byte[0x10];
            gen.GetNonZeroBytes(x);
            string salt = Convert.ToBase64String(x);
            string hash = Convert.ToBase64String(Utils.SHA1(password + salt));

            login.HashedPassword = hash;
            login.Salt = salt;
            login.Flush();
        }

        public RegisterStatus Register(string uuid, string password, bool isGuest, out DbAccount acc)
        {
            acc = null;
            if (!Hashes.SetIfNotExists(0, "logins", uuid.ToUpperInvariant(), "{}").Exec())
                return RegisterStatus.UsedName;

            int newAccId = (int)Strings.Increment(0, "nextAccId").Exec();

            acc = new DbAccount(this, newAccId.ToString())
            {
                UUID = uuid,
                Name = defaultNames[(uint)uuid.GetHashCode() % defaultNames.Length],
                Rank = 0,
                Admin = false,
                NameChosen = false,
                Verified = false,
                Converted = false,
                GuildId = "-1",
                GuildRank = -1,
                VaultCount = 1,
                MaxCharSlot = 3,
                RegTime = DateTime.Now,
                Guest = true,
                Fame = 0,
                TotalFame = 0,
                Credits = 1000,
                FortuneTokens = 0,
                Gifts = new int[] { 0xae9 },
                PetYardType = 0,
                IsAgeVerified = 1,
            };
            acc.Flush();

            var login = new DbLoginInfo(this, uuid);

            var x = new byte[0x10];
            gen.GetNonZeroBytes(x);
            string salt = Convert.ToBase64String(x);
            string hash = Convert.ToBase64String(Utils.SHA1(password + salt));

            login.HashedPassword = hash;
            login.Salt = salt;
            login.AccountId = acc.AccountId;
            login.Flush();

            var stats = new DbClassStats(acc);
            stats.Flush();

            var vault = new DbVault(acc);
            vault[0] = Enumerable.Repeat(-1, 8).ToArray();
            vault.Flush();

            return RegisterStatus.OK;
        }

        public bool HasUUID(string uuid) => Hashes.Exists(0, "login", uuid.ToUpperInvariant()).Exec();

        public DbAccount GetAccountById(string id)
        {
            var ret = new DbAccount(this, id);
            if (ret.IsNull) return null;
            return ret;
        }

        public DbAccount GetAccountByUUID(string uuid)
        {
            var info = new DbLoginInfo(this, uuid);
            if (info.IsNull)
                return null;
            var ret = new DbAccount(this, info.AccountId);
            if (ret.IsNull)
                return null;
            return ret;
        }

        public string ResolveId(string name)
        {
            string val = Hashes.GetString(0, "names", name.ToUpperInvariant()).Exec();
            if (val == null) return "0";
            return val;
        }

        public string ResolveIgn(string accId) => Hashes.GetString(0, $"account.{accId}", "name").Exec();

        public string ResolveIgn(DbAccount acc) => Hashes.GetString(0, $"account.{acc.AccountId}", "name").Exec();

        public void UpdateCredit(DbAccount acc, int amount)
        {
            if (amount > 0)
                WaitAll(Hashes.Increment(0, acc.Key, "credits", amount));
            else
                Hashes.Increment(0, acc.Key, "credits", amount).Wait();
            acc.Flush();
            acc.Reload();
        }

        public void UpdateFame(DbAccount acc, int amount)
        {
            if (amount > 0)
                WaitAll(
                    Hashes.Increment(0, acc.Key, "totalFame", amount),
                    Hashes.Increment(0, acc.Key, "fame", amount));
            else
                Hashes.Increment(0, acc.Key, "fame", amount).Wait();
            acc.Flush();
            acc.Reload();
        }

        public DbClassStats ReadClassStats(DbAccount acc) => new DbClassStats(acc);

        public DbVault ReadVault(DbAccount acc) => new DbVault(acc);

        public int CreateChest(DbVault vault)
        {
            int id = (int)Hashes.Increment(0, vault.Account.Key, "vaultCount").Exec();
            vault[id] = Enumerable.Repeat(-1, 8).ToArray();
            vault.Flush();
            return id;
        }

        public DbChar GetAliveCharacter(DbAccount acc)
        {
            int chara = 1;
            foreach (var i in Sets.GetAll(0, "alive." + acc.AccountId).Exec().Reverse())
                chara = BitConverter.ToInt32(i, 0);
            return LoadCharacter(acc, chara);
        }

        public IEnumerable<int> GetAliveCharacters(DbAccount acc)
        {
            foreach (var i in Sets.GetAll(0, "alive." + acc.AccountId).Exec())
                yield return BitConverter.ToInt32(i, 0);
        }

        public IEnumerable<int> GetDeadCharacters(DbAccount acc)
        {
            foreach (var i in Lists.Range(0, "dead." + acc.AccountId, 0, int.MaxValue).Exec())
                yield return BitConverter.ToInt32(i, 0);
        }

        public bool IsAlive(DbChar character) => Sets.Contains(0, $"alive.{character.Account.AccountId}", BitConverter.GetBytes(character.CharId)).Exec();

        public CreateStatus CreateCharacter(XmlData dat, DbAccount acc, ushort type, int skin, out DbChar character)
        {
            var @class = dat.ObjectTypeToElement[type];

            if (Sets.GetLength(0, "alive." + acc.AccountId).Exec() >= acc.MaxCharSlot)
            {
                character = null;
                return CreateStatus.ReachCharLimit;
            }

            int newId = (int)Hashes.Increment(0, acc.Key, "nextCharId").Exec();
            character = new DbChar(acc, newId)
            {
                ObjectType = type,
                Level = 1,
                Experience = 0,
                Fame = 0,
                HasBackpack = false,
                Items = @class.Element("Equipment").Value.Replace("0xa22", "-1").CommaToArray<int>(),
                Stats = new int[]{
                    int.Parse(@class.Element("MaxHitPoints").Value),
                    int.Parse(@class.Element("MaxMagicPoints").Value),
                    int.Parse(@class.Element("Attack").Value),
                    int.Parse(@class.Element("Defense").Value),
                    int.Parse(@class.Element("Speed").Value),
                    int.Parse(@class.Element("Dexterity").Value),
                    int.Parse(@class.Element("HpRegen").Value),
                    int.Parse(@class.Element("MpRegen").Value),
                },
                HP = int.Parse(@class.Element("MaxHitPoints").Value),
                MP = int.Parse(@class.Element("MaxMagicPoints").Value),
                Tex1 = 0,
                Tex2 = 0,
                Skin = skin,
                Pet = -1,
                FameStats = new byte[0],
                CreateTime = DateTime.Now,
                LastSeen = DateTime.Now
            };
            character.Flush();
            Sets.Add(0, "alive." + acc.AccountId, BitConverter.GetBytes(newId));
            return CreateStatus.OK;
        }

        public DbChar LoadCharacter(DbAccount acc, int charId)
        {
            var ret = new DbChar(acc, charId);
            if (ret.IsNull)
                return null;
            return ret;
        }

        public DbChar LoadCharacter(string accId, int charId)
        {
            var acc = new DbAccount(this, accId);
            if (acc.IsNull)
                return null;
            var ret = new DbChar(acc, charId);
            if (ret.IsNull)
                return null;
            return ret;
        }

        public bool SaveCharacter(DbAccount acc, DbChar character, bool lockAcc)
        {
            using (var trans = CreateTransaction())
            {
                if (lockAcc)
                    trans.AddCondition(Condition.KeyEquals(1,
                        $"lock.{acc.AccountId}", acc.LockToken));
                character.Flush(trans);
                var stats = new DbClassStats(acc);
                stats.Update(character);
                stats.Flush(trans);
                return trans.Execute().Exec();
            }
        }

        public void DeleteCharacter(DbAccount acc, int charId)
        {
            Keys.Remove(0, $"char.{acc.AccountId}.{charId}");
            var buff = BitConverter.GetBytes(charId);
            Sets.Remove(0, $"alive.{acc.AccountId}", buff);
            Lists.Remove(0, $"dead.{acc.AccountId}", buff);
        }

        public void Death(XmlData dat, DbAccount acc, DbChar character, FameStats stats, string killer)
        {
            character.Dead = true;
            SaveCharacter(acc, character, acc.LockToken != null);
            bool firstBorn;
            var finalFame = stats.CalculateTotal(dat, character,
                                new DbClassStats(acc), out firstBorn);

            var death = new DbDeath(acc, character.CharId);
            death.ObjectType = character.ObjectType;
            death.Level = character.Level;
            death.TotalFame = finalFame;
            death.Killer = killer;
            death.FirstBorn = firstBorn;
            death.DeathTime = DateTime.Now;
            death.Flush();

            var idBuff = BitConverter.GetBytes(character.CharId);
            Sets.Remove(0, $"alive.{acc.AccountId}", idBuff);
            Lists.AddFirst(0, $"dead.{acc.AccountId}", idBuff);

            UpdateFame(acc, finalFame);

            var entry = new DbLegendEntry()
            {
                AccId = acc.AccountId,
                ChrId = character.CharId,
                TotalFame = finalFame
            };
            DbLegend.Insert(this, death.DeathTime, entry);
        }

        public void VerifyAge(DbAccount acc)
        {
            Hashes.Set(0, acc.Key, "isAgeVerified", "1");
            acc.Flush();
            acc.Reload();
        }

        public void ChangeClassAvailability(DbAccount acc, XmlData data, ushort type)
        {
            int price;
            if (acc.Credits < (price = data.ObjectDescs[type].UnlockCost))
                return;

            Hashes.Set(0, $"classAvailability.{acc.AccountId}", type.ToString(),
                JsonConvert.SerializeObject(new DbClassAvailabilityEntry()
                {
                    Id = data.ObjectTypeToId[type],
                    Restricted = "unrestricted"
                }));
            UpdateCredit(acc, -price);
            acc.Flush();
            acc.Reload();
        }

        public void AddToFriendList(DbAccount acc, string friendId)
        {

        }
    }
}
