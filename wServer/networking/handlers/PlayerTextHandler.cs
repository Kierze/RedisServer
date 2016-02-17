using wServer.networking.cliPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class PlayerTextHandler : PacketHandlerBase<PlayerTextPacket>
    {
        public override PacketID ID { get { return PacketID.PLAYERTEXT; } }

        protected override void HandlePacket(Client client, PlayerTextPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                if (packet.Text[0] == '/')
                    client.Player.Manager.Commands.Execute(client.Player, t, packet.Text);
                else
                {
                    if (!client.Player.NameChosen)
                    {
                        client.Player.SendInfo("{\"key\":\"server.must_be_named\"}");
                        return;
                    }
                    client.Player.Manager.Chat.Say(client.Player, packet.Text);
                }
            });
        }

    }
}
