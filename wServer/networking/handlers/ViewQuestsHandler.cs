using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class ViewQuestsHandler : PacketHandlerBase<ViewQuestsPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.VIEWQUESTS; }
        }

        protected override void HandlePacket(Client client, ViewQuestsPacket packet)
        {
        }
    }
}
