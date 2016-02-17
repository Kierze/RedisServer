using common;
using System.Linq;
using System.Text;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandListCommand : Command
    {
        //actually the command is 'help', but /help is intercepted by client
        public CommandListCommand() : base("commands") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var sb = new StringBuilder("Available commands: ");
            var cmds = player.Manager.Commands.Commands.Values
                .Where(x => x.HasPermission(player))
                .ToArray();
            for (int i = 0; i < cmds.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(cmds[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    internal class PauseCommand : Command
    {
        public PauseCommand() : base("pause")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.HasConditionEffect(ConditionEffects.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
                return true;
            }
            else
            {
                if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
                {
                    player.SendError("Not safe to pause.");
                    return false;
                }
                else
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("Game paused.");
                    return true;
                }
            }
        }
    }

    internal class ServerCommand : Command
    {
        public ServerCommand() : base("server")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo(player.Owner.Name);
            return true;
        }
    }

    internal class TeleportCommand : Command
    {
        public TeleportCommand() : base("teleport")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Name.EqualsIgnoreCase(args))
            {
                player.SendInfo("You are already at yourself, and always will be!");
                return false;
            }

            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    player.Teleport(time, i.Value.Id);
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }

    internal class TellCommand : Command
    {
        public TellCommand() : base("tell")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }
            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args.Substring(0, index);
            string msg = args.Substring(index + 1);

            if (player.Name.ToLower() == playername.ToLower())
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            if (!player.Manager.Chat.Tell(player, playername, msg))
                player.SendError($"{playername} not found");
            return false;
        }
    }

    internal class CommandTutorial : Command
    {
        public CommandTutorial() : base("tutorial")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array,
            });
            return true;
        }
    }

    internal class WhoCommand : Command
    {
        public WhoCommand() : base("who")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var sb = new StringBuilder("Players online: ");
            var copy = player.Owner.Players.Values.ToArray();
            if (copy.Length == 0)
                player.SendInfo("Nobody else is online");
            else
            {
                for (int i = 0; i < copy.Length; i++)
                {
                    if (i != 0) sb.Append(", ");
                    sb.Append(copy[i].Name);
                }

                player.SendInfo(sb.ToString());
            }
            return true;
        }
    }
}
