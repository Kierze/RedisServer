using System.Collections.Generic;
using wServer.networking.cliPackets;
using wServer.realm;

namespace wServer.networking.handlers
{
    internal class JoinGuildPacketHandler : PacketHandlerBase<JoinGuildPacket>
    {
        public override PacketID ID { get { return PacketID.JOINGUILD; } }

        protected override void HandlePacket(Client client, JoinGuildPacket packet)
        {
        }        
    }
}
