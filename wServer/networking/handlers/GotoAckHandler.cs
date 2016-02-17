using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class GotoAckHandler : PacketHandlerBase<GotoAckPacket>
    {
        public override PacketID ID { get { return PacketID.GOTOACK; } }

        protected override void HandlePacket(Client client, GotoAckPacket packet)
        {
            //TODO: implement something
        }
    }
}
