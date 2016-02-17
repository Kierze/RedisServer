using common;
using System;
using System.Text;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.setpieces;

namespace wServer.realm.commands
{
    internal class OnlineCommand : Command
    {
        public OnlineCommand() : base("online", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var sb = new StringBuilder("Users online: \r\n");
            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Stage == ProtocalStage.Disconnected) continue;
                sb.Append($"{i.Account.Name}#{i.Player.Owner.Name}@{i.Socket.RemoteEndPoint.ToString()}");
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    internal class AnnounceCommand : Command
    {
        public AnnounceCommand() : base("announce", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Announce(args);
            return true;
        }
    }

    internal class GetQuestCommand : Command
    {
        public GetQuestCommand() : base("getQuest", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Quest == null)
            {
                player.SendError("Player does not have a quest!");
                return false;
            }
            player.SendInfo("Quest location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
            return true;
        }
    }

    internal class GiveCommand : Command
    {
        public GiveCommand() : base("give", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ushort objType;
            if (!player.Manager.GameData.IdToObjectType.TryGetValue(args, out objType))
            {
                player.SendError("Unknown item type!");
                return false;
            }
            for (int i = 0; i < player.Inventory.Length; i++)
                if (player.Inventory[i] == null)
                {
                    player.Inventory[i] = player.Manager.GameData.Items[objType];
                    player.UpdateCount++;
                    return true;
                }
            player.SendError("Not enough space in inventory!");
            return false;
        }
    }

    internal class KickCommand : Command
    {
        public KickCommand() : base("kick", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    i.Disconnect();
                    player.SendInfo("Player disconnected!");
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }

    internal class KillAllCommand : Command
    {
        public KillAllCommand() : base("killAll", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int count = 0;
            foreach (var i in player.Owner.Enemies)
            {
                var desc = i.Value.ObjectDesc;
                if (desc != null &&
                    desc.ObjectId != null &&
                    desc.ObjectId.ContainsIgnoreCase(args))
                {
                    i.Value.Death(time);
                    count++;
                }
            }
            player.SendInfo($"{count} enemies killed");
            return true;
        }
    }

    internal class KillPlayerCommand : Command
    {
        public KillPlayerCommand() : base("killPlayer", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    i.Player.HP = 0;
                    i.Player.Death("Moderator");
                    player.SendInfo("Player killed!");
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }

    internal class OryxSayCommand : Command
    {
        public OryxSayCommand() : base("oryxSay", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Oryx(player.Owner, args);
            return true;
        }
    }

    internal class SetpieceCommand : Command
    {
        public SetpieceCommand() : base("setpiece", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                "wServer.realm.setpieces." + args, true, true));
            piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
            return true;
        }
    }

    internal class SpawnCommand : Command
    {
        public SpawnCommand() : base("spawn", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int index = args.IndexOf(' ');
            int num;
            string name = args;

            if (args.IndexOf(' ') > 0 && int.TryParse(args.Substring(0, args.IndexOf(' ')), out num)) //multi
                name = args.Substring(index + 1);
            else
                num = 1;

            ushort objType;
            if (!player.Manager.GameData.IdToObjectType.TryGetValue(name, out objType) ||
                !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
            {
                player.SendError("Unknown entity!");
                return false;
            }

            for (int i = 0; i < num; i++)
            {
                var entity = Entity.Resolve(player.Manager, objType);
                entity.Move(player.X, player.Y);
                player.Owner.EnterWorld(entity);
            }
            return true;
        }
    }

    internal class SummonCommand : Command
    {
        public SummonCommand() : base("summon", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    i.Value.Teleport(time, player.Id);
                    player.SendInfo("Player summoned!");
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }

    internal class ToggleEffectCommand : Command
    {
        public ToggleEffectCommand() : base("eff", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ConditionEffectIndex effect;
            if (!Enum.TryParse(args, true, out effect))
            {
                player.SendError("Invalid effect!");
                return false;
            }
            if ((player.ConditionEffects & (ConditionEffects)(1 << (int)effect)) != 0)
            {
                //remove
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
            else
            {
                //add
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = -1
                });
            }
            return true;
        }
    }

    internal class TpPosCommand : Command
    {
        public TpPosCommand() : base("tpPos", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            string[] coordinates = args.Split(' ');
            if (coordinates.Length != 2)
            {
                player.SendError("Invalid coordinates!");
                return false;
            }

            int x, y;
            if (!int.TryParse(coordinates[0], out x) ||
                !int.TryParse(coordinates[1], out y))
            {
                player.SendError("Invalid coordinates!");
                return false;
            }

            player.Move(x + 0.5f, y + 0.5f);
            player.SetNewbiePeriod();
            player.UpdateCount++;
            player.Owner.BroadcastPacket(new GotoPacket()
            {
                ObjectId = player.Id,
                Position = new Position()
                {
                    X = player.X,
                    Y = player.Y
                }
            }, null);
            return true;
        }
    }

    internal class MaxCommand : Command
    {
        public MaxCommand() : base("max", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            try
            {
                for (int i = 0; i < 8; i++)
                    player.Stats[i] =
                        Int32.Parse(player.Manager.GameData.ObjectTypeToElement[player.ObjectType]
                        .Element(StatsManager.StatsIndexToName(i)).Attribute("max").Value);
                player.UpdateCount++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return true;
        }
    }
}
