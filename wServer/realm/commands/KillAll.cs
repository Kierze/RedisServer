using common;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandKillAll : Command
    {
        public CommandKillAll() : base("killAll", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int count = 0;
            foreach (var i in player.Owner.Enemies)
            {
                var desc = i.Value.ObjectDesc;
                if (desc != null &&
                    desc.ObjectId != null &&
                    desc.ObjectId.ContainsIgnoreCase(args))
                {
                    i.Value.Death(time);
                    count++;
                }
            }
            player.SendInfo($"{count} enemies killed");
            return true;
        }
    }
}
