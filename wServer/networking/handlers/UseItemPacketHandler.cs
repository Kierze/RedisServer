using wServer.networking.cliPackets;

namespace wServer.networking.handlers
{
    internal class UseItemPacketHandler : PacketHandlerBase<UseItemPacket>
    {
        public override PacketID ID { get { return PacketID.USEITEM; } }

        protected override void HandlePacket(Client client, UseItemPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t => client.Player.UseItem(t, packet.SlotObject.ObjectId, packet.SlotObject.SlotId, packet.ItemUsePos));
        }
    }
}
