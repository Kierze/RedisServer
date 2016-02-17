using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class PetYardCommandHandler : PacketHandlerBase<PetYardCommandPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.PETYARDCOMMAND; }
        }

        protected override void HandlePacket(Client client, PetYardCommandPacket packet)
        {
        }
    }
}
