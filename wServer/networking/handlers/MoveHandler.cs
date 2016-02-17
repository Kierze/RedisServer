using common;
using wServer.networking.cliPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class MoveHandler : PacketHandlerBase<MovePacket>
    {
        public override PacketID ID { get { return PacketID.MOVE; } }

        protected override void HandlePacket(Client client, MovePacket packet)
        {
            if (client.Player.Owner == null) return;
            client.Manager.Logic.AddPendingAction(t =>
            {
                client.Player.Flush();

                if (client.Player.HasConditionEffect(ConditionEffects.Paralyzed)) return;
                if (packet.Position.X == -1 || packet.Position.Y == -1) return;

                double newX = client.Player.X;
                double newY = client.Player.Y;

                if (newX != packet.Position.X)
                {
                    newX = packet.Position.X;
                    client.Player.UpdateCount++;
                }
                if (newY != packet.Position.Y)
                {
                    newY = packet.Position.Y;
                    client.Player.UpdateCount++;
                }
                client.Player.Move((float)newX, (float)newY);
            });
        }
    }
}
