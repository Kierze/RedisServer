using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class PlayerHitPacketHandler : PacketHandlerBase<PlayerHitPacket>
    {
        public override PacketID ID { get { return PacketID.PlayerHit; } }

        protected override void HandlePacket(Client client, PlayerHitPacket packet)
        {
            //TODO: implement something
        }
    }
}
