using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandGetQuest : Command
    {
        public CommandGetQuest() : base("getQuest", permLevel: 1)
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
}
