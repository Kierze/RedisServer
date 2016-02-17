#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class EnterArenaPacketHandler : PacketHandlerBase<EnterArenaPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.ENTER_ARENA; }
        }

        protected override void HandlePacket(Client client, EnterArenaPacket packet)
        {
        }
    }
}
