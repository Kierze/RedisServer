using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class TinkerQuestHandler : PacketHandlerBase<TinkerQuestPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.TINKERQUEST; }
        }

        protected override void HandlePacket(Client client, TinkerQuestPacket packet)
        {
        }
    }
}
