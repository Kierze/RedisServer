using common;
using System;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class InvDropPacketHandler : PacketHandlerBase<InvDropPacket>
    {
        public override PacketID ID { get { return PacketID.INVDROP; } }

        protected override void HandlePacket(Client client, InvDropPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                if (client.Player.Owner == null) return;
                Handle(client.Player, packet.SlotObject.SlotId);
            });
        }

        private static Random invRand = new Random();

        private void Handle(Player player, int slot)
        {
            const ushort NORM_BAG = 0x0500;
            const ushort SOUL_BAG = 0x0503;

            IContainer con = player as IContainer;
            if (con.Inventory[slot] == null)
            {
                //still count as dropped
                player.Client.SendPacket(new InvResultPacket() { Result = 0 });
                return;
            }

            Item item = con.Inventory[slot];
            con.Inventory[slot] = null;
            player.UpdateCount++;

            Container container;
            if (item.Soulbound)
            {
                container = new Container(player.Manager, SOUL_BAG, 1000 * 60, true);
                container.BagOwners = new string[] { player.AccountId };
            }
            else
                container = new Container(player.Manager, NORM_BAG, 1000 * 60, true);
            container.Inventory[0] = item;
            container.Move(player.X + (float)((invRand.NextDouble() * 2 - 1) * 0.5),
                           player.Y + (float)((invRand.NextDouble() * 2 - 1) * 0.5));
            container.Size = 75;
            player.Owner.EnterWorld(container);

            player.Client.SendPacket(new InvResultPacket() { Result = 0 });
        }
    }
}
