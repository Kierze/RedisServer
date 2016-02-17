using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;

namespace wServer.networking.handlers
{
    internal class LeaveArenaHandler : PacketHandlerBase<LeaveArenaPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.LEAVEARENA; }
        }

        protected override void HandlePacket(Client client, LeaveArenaPacket packet)
        {
        }
    }
}
