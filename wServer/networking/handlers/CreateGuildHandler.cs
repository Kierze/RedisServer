using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class CreateGuildHandler : PacketHandlerBase<CreateGuildPacket>
    {
        public override PacketID ID { get { return PacketID.CREATEGUILD; } }

        protected override void HandlePacket(Client client, CreateGuildPacket packet)
        {
        }
    }
}
