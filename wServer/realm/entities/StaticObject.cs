using common;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace wServer.realm.entities
{
    public class StaticObject : Entity
    {
        public StaticObject(RealmManager manager, ushort objType, int? lifeTime, bool stat, bool dying, bool hittestable)
            : base(manager, objType, IsInteractive(manager, objType))
        {
            if (Vulnerable = lifeTime.HasValue)
                HP = lifeTime.Value;
            Dying = dying;
            if (objType == 0x01c7)
                Static = true;
            else
                Static = stat;
            Hittestable = hittestable;
        }

        public bool Vulnerable { get; private set; }

        public bool Static { get; private set; }

        public bool Hittestable { get; private set; }

        public int HP { get; set; }

        public bool Dying { get; private set; }

        public static bool GetStatic(XElement elem)
        {
            return elem.Element("Static") != null;
        }

        public static int? GetHP(XElement elem)
        {
            var n = elem.Element("MaxHitPoints");
            if (n != null)
                return Utils.FromString(n.Value);
            return null;
        }

        private static bool IsInteractive(RealmManager manager, ushort objType)
        {
            ObjectDesc desc;
            if (manager.GameData.ObjectDescs.TryGetValue(objType, out desc))
            {
                if (desc.Class != null)
                    if (desc.Class == "Container" || desc.Class.ContainsIgnoreCase("wall") ||
                        desc.Class == "Merchant" || desc.Class == "Portal")
                        return false;
                return !(desc.Static && !desc.Enemy && !desc.EnemyOccupySquare);
            }
            return false;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (!Vulnerable)
                stats[StatsType.HP] = 0;
            else
                stats[StatsType.HP] = HP;
            base.ExportStats(stats);
        }

        protected bool CheckHP()
        {
            try
            {
                if (Vulnerable && HP < 0)
                {
                    if (ObjectDesc != null &&
                        (ObjectDesc.EnemyOccupySquare || ObjectDesc.OccupySquare))
                        if (Owner != null)
                            Owner.Obstacles[(int)(X - 0.5), (int)(Y - 0.5)] = 0;
                    Owner?.LeaveWorld(this);
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        public override void Tick(RealmTime time)
        {
            if (Vulnerable)
            {
                if (Dying)
                {
                    HP -= time.thisTickTimes;
                    UpdateCount++;
                }
                CheckHP();
            }

            base.Tick(time);
        }
    }
}
