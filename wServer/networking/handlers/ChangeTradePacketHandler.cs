using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

namespace wServer.networking.handlers
{
    internal class ChangeTradePacketHandler : PacketHandlerBase<ChangeTradePacket>
    {
        public override PacketID ID { get { return PacketID.ChangeTrade; } }

        protected override void HandlePacket(Client client, ChangeTradePacket packet)
        {
            var player = client.Player;
            player.tradeAccepted = false;
            player.tradeTarget.tradeAccepted = false;
            player.trade = packet.Offers;

            player.tradeTarget.Client.SendPacket(new TradeChangedPacket()
            {
                Offers = player.trade
            });
        }
    }
}
