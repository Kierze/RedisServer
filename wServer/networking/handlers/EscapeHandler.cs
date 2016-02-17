using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;

namespace wServer.networking.handlers
{
    internal class EscapeHandler : PacketHandlerBase<EscapePacket>
    {
        public override PacketID ID { get { return PacketID.ESCAPE; } }

        protected override void HandlePacket(Client client, EscapePacket packet)
        {
            if (client.Player.Owner == null) return;
            var world = client.Manager.GetWorld(client.Player.Owner.Id);
            if (world.Id == World.NEXUS_ID)
            {
                client.SendPacket(new TextPacket
                {
                    Stars = -1,
                    BubbleTime = 0,
                    Name = "",
                    Text = "server.already_nexus"
                });
                return;
            }
            client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = World.NEXUS_ID,
                Name = "Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}
