using common;
using System;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandToggleEffect : Command
    {
        public CommandToggleEffect() : base("eff", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ConditionEffectIndex effect;
            if (!Enum.TryParse(args, true, out effect))
            {
                player.SendError("Invalid effect!");
                return false;
            }
            if ((player.ConditionEffects & (ConditionEffects)(1 << (int)effect)) != 0)
            {
                //remove
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
            else
            {
                //add
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = -1
                });
            }
            return true;
        }
    }
}
