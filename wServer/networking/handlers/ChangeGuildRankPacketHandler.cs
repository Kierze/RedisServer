using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class ChangeGuildRankPacketHandler : PacketHandlerBase<ChangeGuildRankPacket>
    {
        public override PacketID ID { get { return PacketID.CHANGEGUILDRANK; } }

        protected override void HandlePacket(Client client, ChangeGuildRankPacket packet)
        {
        }
    }
}
