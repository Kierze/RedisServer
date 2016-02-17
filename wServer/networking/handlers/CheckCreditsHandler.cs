using wServer.networking.cliPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class CheckCreditsHandler : PacketHandlerBase<CheckCreditsPacket>
    {
        public override PacketID ID { get { return PacketID.CHECKCREDITS; } }

        protected override void HandlePacket(Client client, CheckCreditsPacket packet)
        {
            client.Account.Flush();
            client.Account.Reload();
            client.Manager.Logic.AddPendingAction(t =>
            {
                client.Player.Credits = client.Player.Client.Account.Credits;
                client.Player.UpdateCount++;
            });
        }
    }
}
