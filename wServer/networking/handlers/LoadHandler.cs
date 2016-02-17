using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class LoadHandler : PacketHandlerBase<LoadPacket>
    {
        public override PacketID ID { get { return PacketID.LOAD; } }

        protected override void HandlePacket(Client client, LoadPacket packet)
        {
            client.Character = client.Manager.Database.LoadCharacter(client.Account, packet.CharacterId);
            if (client.Character != null)
            {
                if (client.Character.Dead)
                {
                    SendFailure(client, "Character is dead");
                    client.Disconnect();
                }
                else
                {
                    var target = client.Manager.Worlds[client.targetWorld];
                    client.SendPacket(new CreateSuccessPacket()
                    {
                        CharacterID = client.Character.CharId,
                        ObjectID = target.EnterWorld(client.Player = new Player(client.Manager, client))
                    });
                    client.Stage = ProtocalStage.Ready;
                }
            }
            else
            {
                SendFailure(client, "Failed to load character");
                client.Disconnect();
            }
        }
    }
}
