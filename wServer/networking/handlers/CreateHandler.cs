using common;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class CreateHandler : PacketHandlerBase<CreatePacket>
    {
        public override PacketID ID { get { return PacketID.CREATE; } }

        protected override void HandlePacket(Client client, CreatePacket packet)
        {
            var db = client.Manager.Database;

            DbChar character;
            var status = client.Manager.Database.CreateCharacter(
                client.Manager.GameData, client.Account, (ushort)packet.ClassType, packet.SkinType, out character);

            if (status == CreateStatus.ReachCharLimit)
            {
                SendFailure(client, "Too many characters");
                client.Disconnect();
                return;
            }

            client.Character = character;

            var target = client.Manager.Worlds[client.targetWorld];
            //Delay to let client load remote texture
            target.Timers.Add(new WorldTimer(500, (w, t) =>
            {
                client.SendPacket(new CreateSuccessPacket()
                {
                    CharacterID = client.Character.CharId,
                    ObjectID = target.EnterWorld(client.Player = new Player(client.Manager, client))
                });
            }));
            client.Stage = ProtocalStage.Ready;
        }
    }
}
