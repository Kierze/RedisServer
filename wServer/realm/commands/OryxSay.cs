using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandOryxSay : Command
    {
        public CommandOryxSay() : base("oryxSay", permLevel: 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Oryx(player.Owner, args);
            return true;
        }
    }
}
