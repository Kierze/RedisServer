using common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.logic;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.terrain;

namespace wServer.realm.entities
{
    internal interface IPlayer
    {
        void Damage(int dmg, Entity chr);

        bool IsVisibleToEnemy();
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        private static ILog log = LogManager.GetLogger(nameof(Player));

        private byte[,] tiles;

        private float hpRegenCounter;

        private float mpRegenCounter;

        private bool resurrecting = false;

        public Player(RealmManager manager, Client client) : base(manager, client.Character.ObjectType, client.Random)
        {
            try
            {
                Client = client;
                Manager = client.Manager;
                StatsManager = new StatsManager(this);
                Name = client.Account.Name;
                AccountId = client.Account.AccountId;
                FameCounter = new FameCounter(this);
                Tokens = client.Account.FortuneTokens;
                HpPotionPrice = 5;
                MpPotionPrice = 5;

                Level = client.Character.Level == 0 ? 1 : client.Character.Level;
                Experience = client.Character.Experience;
                ExperienceGoal = GetExpGoal(Level);
                Stars = GetStars();
                Texture1 = client.Character.Tex1;
                Texture2 = client.Character.Tex2;
                Credits = client.Account.Credits;
                NameChosen = client.Account.NameChosen;
                CurrentFame = client.Account.Fame;
                Fame = client.Character.Fame;
                LootDropBoostTimeLeft = client.Character.LootDropTimer;
                LootTierBoostTimeLeft = client.Character.LootTierTimer;
                FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
                Glowing = true;
                Guild = "";
                GuildRank = -1;
                HP = client.Character.HP;
                MP = client.Character.MP;
                ConditionEffects = 0;
                OxygenBar = 100;
                HasBackpack = client.Character.HasBackpack;
                Skin = client.Character.Skin;
                HealthPotions = client.Character.HealthPotions < 0 ? 0 : client.Character.HealthPotions;
                MagicPotions = client.Character.MagicPotions < 0 ? 0 : client.Character.MagicPotions;
                Inventory = new Inventory(this, client.Character.Items
                        .Select(_ => _ == -1 ? null : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                        .ToArray());
                Inventory.InventoryChanged += (sender, e) => CalculateBoost();
                SlotTypes = client.Manager.GameData.ObjectTypeToElement[ObjectType]
                    .Element("SlotTypes").Value.CommaToArray<int>();
                Stats = (int[])client.Character.Stats.Clone();
            } catch(Exception ex)
            {
                log.Error(ex);
            }
            
        }

        //Stats
        public string AccountId { get; set; }

        public int[] Boost { get; private set; }

        public Client Client { get; }

        public int Credits { get; set; }

        public int Tokens { get; set; }

        public int CurrentFame { get; set; }

        public int Experience { get; set; }

        public int ExperienceGoal { get; set; }

        public int Fame { get; set; }

        public FameCounter FameCounter { get; private set; }

        // DailyQuest

        public int FameGoal { get; set; }

        public bool Glowing { get; set; }

        public bool HasBackpack { get; set; }

        public int HealthPotions { get; set; }

        // Ignored

        // Invited

        // Muted

        public int Level { get; set; }

        // Locked

        public bool LootDropBoost
        {
            get { return LootDropBoostTimeLeft > 0; }
            set { LootDropBoostTimeLeft = value ? LootDropBoostTimeLeft : 0.0f; }
        }

        public float LootDropBoostTimeLeft { get; set; }

        public bool LootTierBoost
        {
            get { return LootTierBoostTimeLeft > 0; }
            set { LootTierBoostTimeLeft = value ? LootTierBoostTimeLeft : 0.0f; }
        }

        public float LootTierBoostTimeLeft { get; set; }

        public int MagicPotions { get; set; }

        public ushort HpPotionPrice { get; set; }

        public ushort MpPotionPrice { get; set; }

        public bool HpFirstPurchaseTime { get; set; }

        public bool MpFirstPurchaseTime { get; set; }

        public new RealmManager Manager { get; }

        public int MaxHp { get; set; }

        public int MaxMp { get; set; }

        public int MP { get; set; }

        public bool NameChosen { get; set; }

        public int OxygenBar { get; set; }

        public int Pet { get; set; }

        public int Skin { get; set; }

        public int Stars { get; set; }

        public int[] Stats { get; private set; }

        private StatsManager StatsManager { get; set; }

        public int Texture1 { get; set; }

        public int Texture2 { get; set; }

        public Inventory Inventory { get; private set; }

        public string Guild { get; set; }

        public int GuildRank { get; set; }

        public int[] SlotTypes { get; private set; }

        public void Damage(int dmg, Entity chr)
        {
            try
            {
                if (HasConditionEffect(ConditionEffects.Paused) ||
                    HasConditionEffect(ConditionEffects.Stasis) ||
                    HasConditionEffect(ConditionEffects.Invincible))
                    return;

                dmg = (int)StatsManager.GetDefenseDamage(dmg, false);
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;
                UpdateCount++;
                Owner.BroadcastPacket(new DamagePacket()
                {
                    TargetId = Id,
                    Effects = 0,
                    Damage = (ushort)dmg,
                    Killed = HP <= 0,
                    BulletId = 0,
                    ObjectId = chr.Id
                }, this);

                if (HP <= 0)
                    Death(chr.ObjectDesc.DisplayId ?? chr.ObjectDesc.ObjectId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }            
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId;
            stats[StatsType.Name] = Name;

            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;

            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;

            stats[StatsType.Guild] = Guild;
            stats[StatsType.GuildRank] = GuildRank;

            stats[StatsType.Credits] = Credits;
            stats[StatsType.Tokens] = Tokens;
            stats[StatsType.NameChosen] = NameChosen ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;

            if (Glowing)
                stats[StatsType.Glowing] = 1;

            stats[StatsType.HP] = HP;
            stats[StatsType.MP] = MP;

            stats[StatsType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.Inventory11] = Inventory[11]?.ObjectType ?? -1;

            if (Boost == null) CalculateBoost();

            if (Boost != null)
            {
                stats[StatsType.MaximumHP] = Stats[0] + Boost[0];
                stats[StatsType.MaximumMP] = Stats[1] + Boost[1];
                stats[StatsType.Attack] = Stats[2] + Boost[2];
                stats[StatsType.Defense] = Stats[3] + Boost[3];
                stats[StatsType.Speed] = Stats[4] + Boost[4];
                stats[StatsType.Vitality] = Stats[5] + Boost[5];
                stats[StatsType.Wisdom] = Stats[6] + Boost[6];
                stats[StatsType.Dexterity] = Stats[7] + Boost[7];

                stats[StatsType.HPBoost] = Boost[0];
                stats[StatsType.MPBoost] = Boost[1];
                stats[StatsType.AttackBonus] = Boost[2];
                stats[StatsType.DefenseBonus] = Boost[3];
                stats[StatsType.SpeedBonus] = Boost[4];
                stats[StatsType.VitalityBonus] = Boost[5];
                stats[StatsType.WisdomBonus] = Boost[6];
                stats[StatsType.DexterityBonus] = Boost[7];
            }

            stats[StatsType.Size] = Size;
            stats[StatsType.Has_Backpack] = HasBackpack.GetHashCode();
            stats[StatsType.Backpack0] = HasBackpack ? (Inventory[12]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack1] = HasBackpack ? (Inventory[13]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack2] = HasBackpack ? (Inventory[14]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack3] = HasBackpack ? (Inventory[15]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack4] = HasBackpack ? (Inventory[16]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack5] = HasBackpack ? (Inventory[17]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack6] = HasBackpack ? (Inventory[18]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack7] = HasBackpack ? (Inventory[19]?.ObjectType ?? -1) : -1;

            stats[StatsType.Skin] = Skin;
            stats[StatsType.HealStackCount] = HealthPotions;
            stats[StatsType.MagicStackCount] = MagicPotions;

            if (Owner != null && Owner.Name == "Ocean Trench")
                stats[StatsType.OxygenBar] = OxygenBar;

            //stats[StatsType.XpBoosterActive] = XpBoosted ? 1 : 0;
            //stats[StatsType.XpBoosterTime] = (int)XpBoostTimeLeft;
            stats[StatsType.LootDropBoostTimer] = (int)LootDropBoostTimeLeft;
            stats[StatsType.LootTierBoostTimer] = (int)LootTierBoostTimeLeft;
        }

        private void CalculateBoost()
        {
            if (Boost == null) Boost = new int[12];
            else
                for (int i = 0; i < Boost.Length; i++) Boost[i] = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Inventory.Length < i || Inventory.Length == 0) return;
                if (Inventory[i] == null) continue;
                foreach (var b in Inventory[i].StatsBoost)
                {
                    switch ((StatsType)b.Key)
                    {
                        case StatsType.MaximumHP: Boost[0] += b.Value; break;
                        case StatsType.MaximumMP: Boost[1] += b.Value; break;
                        case StatsType.Attack: Boost[2] += b.Value; break;
                        case StatsType.Defense: Boost[3] += b.Value; break;
                        case StatsType.Speed: Boost[4] += b.Value; break;
                        case StatsType.Vitality: Boost[5] += b.Value; break;
                        case StatsType.Wisdom: Boost[6] += b.Value; break;
                        case StatsType.Dexterity: Boost[7] += b.Value; break;
                    }
                }
            }
        }

        public void Death(string killer)
        {
            if (Client.Stage == ProtocalStage.Disconnected || resurrecting)
                return;
            if (CheckResurrection())
                return;

            GenerateGravestone();
            foreach (var i in Owner.Players.Values)
                i.SendInfo(Name + " died at Level " + Level + ", with " + Fame + " Fame" +/* " and " + Experience + " Experience " + */", killed by " + killer); //removed XP as max packet length reached!

            SaveToCharacter();
            Manager.Database.SaveCharacter(Client.Account, Client.Character, true);
            Manager.Database.Death(Manager.GameData, Client.Account, Client.Character, FameCounter.Stats, killer);
            Client.SendPacket(new DeathPacket()
            {
                AccountId = AccountId,
                CharId = Client.Character.CharId,
                Killer = killer,
            });
            Owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
            Owner.LeaveWorld(this);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible))
                return false;

            var dmg = (int)StatsManager.GetDefenseDamage(projectile.Damage, projectile.Descriptor.ArmorPiercing);
            if (!HasConditionEffect(ConditionEffects.Invulnerable))
                HP -= dmg;
            ApplyConditionEffect(projectile.Descriptor.Effects);
            UpdateCount++;
            Owner.BroadcastPacket(new DamagePacket()
            {
                TargetId = Id,
                Effects = HasConditionEffect(ConditionEffects.Invincible) ? 0 : projectile.ConditionEffects,
                Damage = (ushort)dmg,
                Killed = HP <= 0,
                BulletId = projectile.ProjectileId,
                ObjectId = projectile.ProjectileOwner.Self.Id
            }, this);

            if (HP <= 0)
                Death(
       projectile.ProjectileOwner.Self.ObjectDesc.DisplayId ??
       projectile.ProjectileOwner.Self.ObjectDesc.ObjectId);

            return base.HitByProjectile(projectile, time);
        }

        public override void Init(World owner)
        {
            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(0, owner.Map.Width);
                y = rand.Next(0, owner.Map.Height);
            } while (owner.Map[x, y].Region != TileRegion.Spawn);
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];
            SetNewbiePeriod();

            
            

            if (owner.Id == World.NEXUS_ID || owner.Name == "Vault")
            {
                Client.SendPacket(new GlobalNotificationPacket
                {
                    Type = 0,
                    Text = Client.Account.Gifts.Count() > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });
            }

            base.Init(owner);
        }

        public void SaveToCharacter()
        {
            var chr = Client.Character;
            chr.Experience = Experience;
            chr.Level = Level;
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            // Pet
            chr.Fame = Fame;
            chr.HP = HP;
            chr.MP = MP;
            chr.Items = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
            chr.Stats = Stats;
            chr.HealthPotions = HealthPotions;
            chr.MagicPotions = MagicPotions;
            chr.HasBackpack = HasBackpack;
            chr.Skin = Skin;
            //chr.XPBoosted = XpBoosted;
            //chr.XPBoostTimer = (int)XpBoostTimeLeft;
            chr.LootDropTimer = (int)LootDropBoostTimeLeft;
            chr.LootTierTimer = (int)LootTierBoostTimeLeft;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
        }

        public void Teleport(RealmTime time, int objId)
        {
            if (!TPCooledDown())
            {
                SendError("Too soon to teleport again!");
                return;
            }
            SetTPDisabledPeriod();
            var obj = Owner.GetEntity(objId);
            if (obj == null) return;
            Move(obj.X, obj.Y);
            FameCounter.Teleport();
            SetNewbiePeriod();
            UpdateCount++;
            Owner.BroadcastPacket(new GotoPacket()
            {
                ObjectId = Id,
                Position = new Position()
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastPacket(new ShowEffectPacket()
            {
                EffectType = EffectType.Teleport,
                TargetId = Id,
                PosA = new Position()
                {
                    X = X,
                    Y = Y
                },
                Color = new ARGB(0xFFFFFFFF)
            }, null);
        }

        public override void Tick(RealmTime time)
        {
            if (Client.Stage == ProtocalStage.Disconnected)
            {
                Owner.LeaveWorld(this);
                return;
            }
            if (!KeepAlive(time)) return;

            if (Boost == null) CalculateBoost();

            CheckTradeTimeout(time);
            HandleRegen(time);
            HandleQuest(time);
            HandleGround(time);
            HandleEffects(time);
            FameCounter.Tick(time);

            SendUpdate(time);

            if (HP <= 0)
            {
                Death("Unknown");
                return;
            }

            base.Tick(time);
        }

        private bool CheckResurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];
                if (item == null || !item.Resurrects) continue;

                HP = Stats[0] + Stats[0];
                MP = Stats[1] + Stats[1];
                Inventory[i] = null;
                foreach (var player in Owner.Players.Values)
                    player.SendInfo($"{Name}'s {item.DisplayId ?? item.ObjectId} breaks and he dissappears");

                Client.Reconnect(new ReconnectPacket()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array,
                });

                resurrecting = true;
                return true;
            }
            return false;
        }

        private void GenerateGravestone()
        {
            int maxed = 0;
            foreach (var i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
            {
                int limit = int.Parse(Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value).Attribute("max").Value);
                int idx = StatsManager.StatsNameToIndex(i.Value);
                if (Stats[idx] >= limit)
                    maxed++;
            }

            ushort objType;
            int? time;
            switch (maxed)
            {
                case 8:
                    objType = 0x0735; time = null;
                    break;
                case 7:
                    objType = 0x0734; time = null;
                    break;
                case 6:
                    objType = 0x072b; time = null;
                    break;
                case 5:
                    objType = 0x072a; time = null;
                    break;
                case 4:
                    objType = 0x0729; time = null;
                    break;
                case 3:
                    objType = 0x0728; time = null;
                    break;
                case 2:
                    objType = 0x0727; time = null;
                    break;
                case 1:
                    objType = 0x0726; time = null;
                    break;
                default:
                    if (Level <= 1)
                    {
                        objType = 0x0723; time = 30 * 1000;
                    }
                    else if (Level < 20)
                    {
                        objType = 0x0724; time = 60 * 1000;
                    }
                    else
                    {
                        objType = 0x0725; time = 5 * 60 * 1000;
                    }
                    break;
            }
            StaticObject obj = new StaticObject(Manager, objType, time, true, time == null ? false : true, false);
            obj.Move(X, Y);
            obj.Name = Name;
            Owner.EnterWorld(obj);
        }

        private void HandleRegen(RealmTime time)
        {
            if (HP == Stats[0] + Boost[0] || !CanHpRegen())
                hpRegenCounter = 0;
            else
            {
                hpRegenCounter += StatsManager.GetHPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + regen);
                    hpRegenCounter -= regen;
                    UpdateCount++;
                }
            }

            if (MP == Stats[1] + Boost[1] || !CanMpRegen())
                mpRegenCounter = 0;
            else
            {
                mpRegenCounter += StatsManager.GetMPRegen() * time.thisTickTimes / 1000f;
                int regen = (int)mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1] + Boost[1], MP + regen);
                    mpRegenCounter -= regen;
                    UpdateCount++;
                }
            }
        }

        protected override void ImportStats(StatsType stats, object val)
        {
            base.ImportStats(stats, val);
            switch (stats)
            {
                case StatsType.AccountId: AccountId = (string)val; break;

                case StatsType.Experience: Experience = (int)val; break;
                case StatsType.ExperienceGoal: ExperienceGoal = (int)val; break;
                case StatsType.Level: Level = (int)val; break;

                case StatsType.Fame: CurrentFame = (int)val; break;
                case StatsType.CurrentFame: Fame = (int)val; break;
                case StatsType.FameGoal: FameGoal = (int)val; break;
                case StatsType.Stars: Stars = (int)val; break;

                case StatsType.Guild: Guild = (string)val; break;
                case StatsType.GuildRank: GuildRank = (int)val; break;

                case StatsType.Credits: Credits = (int)val; break;
                case StatsType.NameChosen: NameChosen = (int)val != 0 ? true : false; break;
                case StatsType.Texture1: Texture1 = (int)val; break;
                case StatsType.Texture2: Texture2 = (int)val; break;

                case StatsType.Glowing: Glowing = (int)val != 0 ? true : false; break;
                case StatsType.HP: HP = (int)val; break;
                case StatsType.MP: MP = (int)val; break;

                case StatsType.Inventory0: Inventory[0] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory1: Inventory[1] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory2: Inventory[2] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory3: Inventory[3] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory4: Inventory[4] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory5: Inventory[5] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory6: Inventory[6] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory7: Inventory[7] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory8: Inventory[8] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory9: Inventory[9] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory10: Inventory[10] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;
                case StatsType.Inventory11: Inventory[11] = (int)val == -1 ? null : Manager.GameData.Items[(ushort)(int)val]; break;

                case StatsType.MaximumHP: Stats[0] = (int)val; break;
                case StatsType.MaximumMP: Stats[1] = (int)val; break;
                case StatsType.Attack: Stats[2] = (int)val; break;
                case StatsType.Defense: Stats[3] = (int)val; break;
                case StatsType.Speed: Stats[4] = (int)val; break;
                case StatsType.Vitality: Stats[5] = (int)val; break;
                case StatsType.Wisdom: Stats[6] = (int)val; break;
                case StatsType.Dexterity: Stats[7] = (int)val; break;
            }
        }
    }
}
