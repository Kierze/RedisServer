using common;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds;

namespace wServer.networking.handlers
{
    internal class InvSwapHandler : PacketHandlerBase<InvSwapPacket>
    {
        public override PacketID ID { get { return PacketID.INVSWAP; } }

        protected override void HandlePacket(Client client, InvSwapPacket packet)
        {
            if (client.Player.Owner == null) return;

            client.Manager.Logic.AddPendingAction(t =>
            {
                var en1 = client.Player.Owner.GetEntity(packet.SlotObject1.ObjectId);
                var en2 = client.Player.Owner.GetEntity(packet.SlotObject2.ObjectId);
                var con1 = en1 as IContainer;
                var con2 = en2 as IContainer;

                if (packet.SlotObject1.SlotId == 254 ||
                    packet.SlotObject1.SlotId == 255 ||
                    packet.SlotObject2.SlotId == 254 ||
                    packet.SlotObject2.SlotId == 255)
                {
                    if (packet.SlotObject2.SlotId == 254)
                        if (client.Player.HealthPotions < 6)
                        {
                            client.Player.HealthPotions++;
                            con1.Inventory[packet.SlotObject1.SlotId] = null;
                        }
                    if (packet.SlotObject2.SlotId == 255)
                        if (client.Player.MagicPotions < 6)
                        {
                            client.Player.MagicPotions++;
                            con1.Inventory[packet.SlotObject1.SlotId] = null;
                        }
                    if (packet.SlotObject1.SlotId == 254)
                        if (client.Player.HealthPotions > 0)
                        {
                            client.Player.HealthPotions--;
                            con2.Inventory[packet.SlotObject2.SlotId] = null;
                        }
                    if (packet.SlotObject1.SlotId == 255)
                        if (client.Player.MagicPotions > 0)
                        {
                            client.Player.MagicPotions--;
                            con2.Inventory[packet.SlotObject1.SlotId] = null;
                        }
                    if (en1 is Player)
                        (en1 as Player).Client.SendPacket(new InvResultPacket { Result = 0 });
                    else if (en2 is Player)
                        (en2 as Player).Client.SendPacket(new InvResultPacket { Result = 0 });
                    return;
                }
 
                var item1 = con1.Inventory[packet.SlotObject1.SlotId];
                var item2 = con2.Inventory[packet.SlotObject2.SlotId];
                var publicBags = new List<ushort>
                {
                    0x0500,
                    0x0506,
                    0x0501
                };

                if (en1.Dist(en2) > 1)
                {
                    if (en1 is Player)
                        (en1 as Player).Client.SendPacket(new InvResultPacket
                        {
                            Result = -1
                        });
                    else if (en2 is Player)
                        (en2 as Player).Client.SendPacket(new InvResultPacket
                        {
                            Result = -1
                        });
                    en1.UpdateCount++;
                    en2.UpdateCount++;
                    return;
                }

                if (!isValid(item1, item2, con1, con2, packet, client))
                {
                    client.Disconnect();
                    return;
                }

                if (en1 is Player && en2 is Player & en1.Id != en2.Id)
                    return;

                con1.Inventory[packet.SlotObject1.SlotId] = item2;
                con2.Inventory[packet.SlotObject2.SlotId] = item1;

                if (item2 != null)
                {
                    if (publicBags.Contains(en1.ObjectType) && item2.Soulbound)
                    {
                        client.Player.DropBag(item2);
                        con1.Inventory[packet.SlotObject1.SlotId] = null;
                    }
                }
                if (item1 != null)
                {
                    if (publicBags.Contains(en2.ObjectType) && item1.Soulbound)
                    {
                        client.Player.DropBag(item1);
                        con2.Inventory[packet.SlotObject2.SlotId] = null;
                    }
                }

                en1.UpdateCount++;
                en2.UpdateCount++;

                if (en1 is Player)
                {
                    if (en1.Owner.Name == "Vault")
                        (en1 as Player).Client.Save();
                    (en1 as Player).CalculateBoost();
                    (en1 as Player).Client.SendPacket(new InvResultPacket { Result = 0 });
                }
                if (en2 is Player)
                {
                    if (en2.Owner.Name == "Vault")
                        (en2 as Player).Client.Save();
                    (en2 as Player).CalculateBoost();
                    (en2 as Player).Client.SendPacket(new InvResultPacket { Result = 0 });
                }

                client.Player.SaveToCharacter();
                client.Save();
            });
        }

        private bool isValid(Item item1, Item item2, IContainer con1, IContainer con2, InvSwapPacket packet, Client client)
        {
            if (con2 is Container)
                return true;

            bool ret = false;

            if (con1 is Container)
                ret = con2.AuditItem(item1, packet.SlotObject2.SlotId);

            if (con1 is Player && con2 is Player)
                ret = con1.AuditItem(item1, packet.SlotObject2.SlotId) && con2.AuditItem(item2, packet.SlotObject1.SlotId);

            return ret;
        }
    }
}
