using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class GuildRemovePacketHandler : PacketHandlerBase<GuildRemovePacket>
    {
        public override PacketID ID { get { return PacketID.GUILDREMOVE; } }

        protected override void HandlePacket(Client client, GuildRemovePacket packet)
        {
        }
    }
}
