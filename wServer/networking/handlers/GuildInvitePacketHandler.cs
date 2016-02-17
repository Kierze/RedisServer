using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class GuildInvitePacketHandler : PacketHandlerBase<GuildInvitePacket>
    {
        public override PacketID ID { get { return PacketID.GUILDINVITE; } }

        protected override void HandlePacket(Client client, GuildInvitePacket packet)
        {
        }
    }
}
