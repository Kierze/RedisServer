using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

namespace wServer.networking.handlers
{
    internal class PlayerShootHandler : PacketHandlerBase<PlayerShootPacket>
    {
        public override PacketID ID { get { return PacketID.PLAYERSHOOT; } }

        protected override void HandlePacket(Client client, PlayerShootPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                var item = client.Player.Manager.GameData.Items[(ushort)packet.ContainerType];
                int stype = 0;

                for (int i = 0; i < 4; i++)
                    if (client.Player.Inventory[i]?.ObjectType == packet.ContainerType)
                    {
                        stype = i;
                        break;
                    }

                if (client.Player.SlotTypes[stype] != item.SlotType)
                {
                    client.Disconnect();
                    return;
                }

                var prjDesc = item.Projectiles[0]; //Assume only one
                var prj = client.Player.PlayerShootProjectile(
                    packet.BulletId, prjDesc, item.ObjectType,
                    packet.Time, packet.Position, packet.Angle);
                client.Player.Owner.EnterWorld(prj);
                client.Player.BroadcastSync(new AllyShootPacket()
                {
                    OwnerId = client.Player.Id,
                    Angle = packet.Angle,
                    ContainerType = packet.ContainerType,
                    BulletId = packet.BulletId
                }, p => p != client.Player && client.Player.Dist(p) < 25);
                client.Player.FameCounter.Shoot(prj);
            });
        }
    }
}
