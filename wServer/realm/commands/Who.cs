using System.Linq;
using System.Text;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandWho : Command
    {
        public CommandWho() : base("who")
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
