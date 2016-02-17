using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class PetCommandHandler : PacketHandlerBase<PetCommandPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.PETCOMMAND; }
        }

        protected override void HandlePacket(Client client, PetCommandPacket packet)
        {
        }
    }
}
