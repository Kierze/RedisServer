using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class RequestTradeHandler : PacketHandlerBase<RequestTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.REQUESTTRADE; }
        }

        protected override void HandlePacket(Client client, RequestTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.RequestTrade(packet.Name));
        }
    }

    internal class CancelTradeHandler : PacketHandlerBase<CancelTradePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CANCELTRADE; }
        }

        protected override void HandlePacket(Client client, CancelTradePacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => client.Player.CancelTrade());
        }
    }
}
