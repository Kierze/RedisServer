using System.Linq;
using System.Text;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandHelp : Command
    {
        //actually the command is 'help', but /help is intercepted by client
        public CommandHelp() : base("commands") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            StringBuilder sb = new StringBuilder("Available commands: ");
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
}
