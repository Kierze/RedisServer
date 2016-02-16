using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class CancelTradePacketHandler : PacketHandlerBase<CancelTradePacket>
    {
        public override PacketID ID { get { return PacketID.CANCELTRADE; } }

        protected override void HandlePacket(Client client, CancelTradePacket packet)
        {
            client.Player.CancelTrade();
        }
    }
}
