using common;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class InvSwapPacketHandler : PacketHandlerBase<InvSwapPacket>
    {
        public override PacketID ID { get { return PacketID.INVSWAP; } }

        protected override void HandlePacket(Client client, InvSwapPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;
                Handle(
                    client.Player,
                    client.Player.Owner.GetEntity(packet.SlotObject1.ObjectId),
                    client.Player.Owner.GetEntity(packet.SlotObject2.ObjectId),
                    packet.SlotObject1.SlotId, packet.SlotObject2.SlotId);
            });
        }

        private void Handle(Player player,
                    Entity a, Entity b,
                    int slotA, int slotB)
        {
            IContainer conA = a as IContainer;
            IContainer conB = b as IContainer;
            if (player == null || conA == null || conB == null)
            {
                player.Client.SendPacket(new InvResultPacket() { Result = 1 });
                return;
            }

            Item itemA = conA.Inventory[slotA];
            Item itemB = conB.Inventory[slotB];

            if (!conB.AuditItem(itemA, slotB) ||
                !conA.AuditItem(itemB, slotB))
                player.Client.SendPacket(new InvResultPacket() { Result = 1 });
            else
            {
                conA.Inventory[slotA] = itemB;
                conB.Inventory[slotB] = itemA;
                a.UpdateCount++;
                b.UpdateCount++;

                player.Client.SendPacket(new InvResultPacket() { Result = 0 });
            }
        }
    }
}
