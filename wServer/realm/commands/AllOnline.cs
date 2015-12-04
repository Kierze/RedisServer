using System.Text;
using wServer.networking;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandAllOnline : Command
    {
        public CommandAllOnline() : base("online", 1)
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
}
