using common;
using wServer.realm.terrain;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private long l = 0;

        private void HandleGround(RealmTime time)
        {
            if (time.tickTimes - l > 500)
            {
                if (HasConditionEffect(ConditionEffects.Paused) ||
                    HasConditionEffect(ConditionEffects.Invincible))
                    return;

                WmapTile tile = Owner.Map[(int)X, (int)Y];
                ObjectDesc objDesc = tile.ObjType == 0 ? null : Manager.GameData.ObjectDescs[tile.ObjType];
                TileDesc tileDesc = Manager.GameData.Tiles[tile.TileId];
                if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
                {
                    int dmg = Random.Next(tileDesc.MinDamage, tileDesc.MaxDamage);
                    dmg = (int)statsMgr.GetDefenseDamage(dmg, true);

                    HP -= dmg;
                    UpdateCount++;
                    if (HP <= 0)
                        Death("lava");

                    l = time.tickTimes;
                }
            }
        }
    }
}
