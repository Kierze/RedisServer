using common;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandKick : Command
    {
        public CommandKick() : base("kick", permLevel: 1)
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
            player.SendError(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }
}
