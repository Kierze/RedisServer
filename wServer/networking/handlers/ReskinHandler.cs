#region

using wServer.networking.cliPackets;
using wServer.realm;

#endregion

namespace wServer.networking.handlers
{
    internal class ReskinHandler : PacketHandlerBase<ReskinPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.RESKIN; }
        }

        protected override void HandlePacket(Client client, ReskinPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                if (packet.SkinId == 0)
                    client.Player.Skin = 0;
                else
                    client.Player.Skin = packet.SkinId;
                client.Player.UpdateCount++;
                client.Player.SaveToCharacter();
                client.Save();
            });
        }
    }
}
