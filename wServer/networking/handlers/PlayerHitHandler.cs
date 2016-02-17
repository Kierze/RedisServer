using common;
using System;
using wServer.networking.cliPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class PlayerHitHandler : PacketHandlerBase<PlayerHitPacket>
    {
        public override PacketID ID { get { return PacketID.PLAYERHIT; } }

        protected override void HandlePacket(Client client, PlayerHitPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                try
                {
                    if (client.Player.Owner != null)
                    {
                        Projectile proj;
                        if (
                            client.Player.Owner.Projectiles.TryGetValue(
                                new Tuple<int, byte>(packet.ObjectId, packet.BulletId), out proj))
                        {
                            foreach (ConditionEffect effect in proj.Descriptor.Effects)
                                client.Player.ApplyConditionEffect(effect);
                            client.Player.Damage(proj.Damage, proj.ProjectileOwner.Self);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }
    }
}
