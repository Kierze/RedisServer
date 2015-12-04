using common;
using System.Linq;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class PauseCommand : Command
    {
        public PauseCommand() : base("pause")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.HasConditionEffect(ConditionEffects.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
                return true;
            }
            else
            {
                if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
                {
                    player.SendError("Not safe to pause.");
                    return false;
                }
                else
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("Game paused.");
                    return true;
                }
            }
        }
    }
}
