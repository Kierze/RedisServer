using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class EnemyHitPacketHandler : PacketHandlerBase<EnemyHitPacket>
    {
        public override PacketID ID { get { return PacketID.ENEMYHIT; } }

        protected override void HandlePacket(Client client, EnemyHitPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
                Handle(client.Player, t, packet.TargetId, packet.BulletId));
        }

        private void Handle(Player player, RealmTime time, int targetId, byte bulletId)
        {
            if (player.Owner == null) return;
            var entity = player.Owner.GetEntity(targetId);
            if (entity != null)   //Tolerance
            {
                var prj = (player as IProjectileOwner).Projectiles[bulletId];
                if (prj != null)
                    prj.ForceHit(entity, time);
            }
        }
    }
}
