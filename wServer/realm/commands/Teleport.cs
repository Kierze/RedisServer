using common;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandTeleport : Command
    {
        public CommandTeleport() : base("teleport")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Name.EqualsIgnoreCase(args))
            {
                player.SendInfo("You are already at yourself, and always will be!");
                return false;
            }

            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    player.Teleport(time, i.Value.Id);
                    return true;
                }
            }
            player.SendInfo($"Player \"{args}\" could not be found");
            return false;
        }
    }
}
