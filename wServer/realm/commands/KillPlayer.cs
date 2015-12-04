using common;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandKillPlayer : Command
    {
        public CommandKillPlayer() : base("killPlayer", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    i.Player.HP = 0;
                    i.Player.Death("Moderator");
                    player.SendInfo("Player killed!");
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }
}
