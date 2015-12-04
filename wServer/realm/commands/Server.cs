using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandServer : Command
    {
        public CommandServer() : base("server")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo(player.Owner.Name);
            return true;
        }
    }
}
