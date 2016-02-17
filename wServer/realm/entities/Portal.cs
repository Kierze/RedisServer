﻿using common;
using System.Collections.Generic;

namespace wServer.realm.entities
{
    public class Portal : StaticObject
    {
        public Portal(RealmManager manager, ushort objType, int? life)
            : base(manager, objType, life, false, true, false)
        {
            Usable = objType != 0x0721;
        }

        public World WorldInstance { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (ObjectType != 0x072f)
                stats[StatsType.PortalUsable] = Usable ? 1 : 0;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }

        public bool Usable { get; set; }
    }
}
