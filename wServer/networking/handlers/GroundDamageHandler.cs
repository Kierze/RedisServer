using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class GroundDamageHandler : PacketHandlerBase<GroundDamagePacket>
    {
        public override PacketID ID { get { return PacketID.GROUNDDAMAGE; } }

        protected override void HandlePacket(Client client, GroundDamagePacket packet)
        {
            //TODO: implement something
        }
    }
}
