using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class EditAccountListPacketHandler : PacketHandlerBase<EditAccountListPacket>
    {
        public override PacketID ID { get { return PacketID.EDITACCOUNTLIST; } }

        protected override void HandlePacket(Client client, EditAccountListPacket packet)
        {
            //TODO: implement something
        }
    }
}
