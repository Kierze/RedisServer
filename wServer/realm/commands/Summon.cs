using common;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandSummon : Command
    {
        public CommandSummon() : base("summon", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    i.Value.Teleport(time, player.Id);
                    player.SendInfo("Player summoned!");
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }
}
