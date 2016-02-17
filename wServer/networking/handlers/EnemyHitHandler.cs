using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class EnemyHitHandler : PacketHandlerBase<EnemyHitPacket>
    {
        public override PacketID ID { get { return PacketID.ENEMYHIT; } }

        protected override void HandlePacket(Client client, EnemyHitPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                var entity = client.Player.Owner.GetEntity(packet.TargetId);
                if (entity != null)   //Tolerance
                {
                    var prj = (client.Player as IProjectileOwner).Projectiles[packet.BulletId];
                    if (prj != null)
                        prj.ForceHit(entity, t);
                }
            });
        }
    }
}
