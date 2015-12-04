using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandAnnouncement : Command
    {
        public CommandAnnouncement() : base("announce", permLevel: 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Announce(args);
            return true;
        }
    }
}
