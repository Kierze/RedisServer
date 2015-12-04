using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandTpPos : Command
    {
        public CommandTpPos() : base("tpPos", permLevel: 1)
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
}
