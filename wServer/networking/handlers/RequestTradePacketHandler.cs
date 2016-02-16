using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class RequestTradePacketHandler : PacketHandlerBase<RequestTradePacket>
    {
        public override PacketID ID { get { return PacketID.REQUESTTRADE; } }

        protected override void HandlePacket(Client client, RequestTradePacket packet)
        {
            client.Player.RequestTrade(packet.Name);
        }
    }
}
