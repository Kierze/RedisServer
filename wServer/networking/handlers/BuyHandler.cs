using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class BuyHandler : PacketHandlerBase<BuyPacket>
    {
        public override PacketID ID { get { return PacketID.BUY; } }

        protected override void HandlePacket(Client client, BuyPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                var obj = client.Player?.Owner?.GetEntity(packet.ObjectId) as SellableObject;
                if (obj == null)
                {
                    client.SendPacket(new BuyResultPacket
                    {
                        Result = -1,
                        Message = "Invalid Object"
                    });
                    return;
                }
                for (var i = 0; i < packet.Quantity; i++)
                    obj.Buy(client.Player);
            });
        }
    }
}
