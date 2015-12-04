using common;
using System;

namespace wServer.realm.entities
{
    partial class Player
    {
        private float healing = 0;
        private float bleeding = 0;
        private int healCount = 0;

        private void HandleEffects(RealmTime time)
        {
            if (HasConditionEffect(ConditionEffects.Healing))
            {
                if (healing > 1)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + (int)healing);
                    healing -= (int)healing;
                    UpdateCount++;
                    healCount++;
                }
                healing += 28 * (time.thisTickTimes / 1000f);
            }
            if (HasConditionEffect(ConditionEffects.Quiet) &&
                MP > 0)
            {
                MP = 0;
                UpdateCount++;
            }
            if (HasConditionEffect(ConditionEffects.Bleeding) &&
                HP > 1)
            {
                if (bleeding > 1)
                {
                    HP -= (int)bleeding;
                    bleeding -= (int)bleeding;
                    UpdateCount++;
                }
                bleeding += 28 * (time.thisTickTimes / 1000f);
            }

            if (newbieTime > 0)
            {
                newbieTime -= time.thisTickTimes;
                if (newbieTime < 0)
                    newbieTime = 0;
            }
            if (CanTPCooldownTime > 0)
            {
                CanTPCooldownTime -= time.thisTickTimes;
                if (CanTPCooldownTime < 0)
                    CanTPCooldownTime = 0;
            }
        }

        private bool CanHpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Sick))
                return false;
            if (HasConditionEffect(ConditionEffects.Bleeding))
                return false;
            return true;
        }

        private bool CanMpRegen()
        {
            if (HasConditionEffect(ConditionEffects.Quiet))
                return false;
            return true;
        }

        private int newbieTime = 0;

        internal void SetNewbiePeriod()
        {
            newbieTime = 3000;
        }

        private int CanTPCooldownTime = 0;

        internal void SetTPDisabledPeriod()
        {
            CanTPCooldownTime = 10 * 1000; // 10 seconds
        }

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffects.Paused))
                return false;
            if (HasConditionEffect(ConditionEffects.Invisible))
                return false;
            if (newbieTime > 0)
                return false;
            return true;
        }

        public bool TPCooledDown()
        {
            if (CanTPCooldownTime > 0)
                return false;
            return true;
        }
    }
}
