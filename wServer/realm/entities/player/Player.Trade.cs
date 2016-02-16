using System;
using System.Collections.Generic;
using wServer.networking.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        internal Dictionary<Player, int> potentialTrader = new Dictionary<Player, int>();
        internal Player tradeTarget;
        internal bool[] trade;
        internal bool tradeAccepted;

        public void RequestTrade(string name)
        {
            if (!NameChosen)
            {
                SendError("Unique name is required to trade with other!");
                return;
            }
            if (tradeTarget != null)
            {
                SendError("You're already trading!");
                return;
            }
            Player target = Owner.GetUniqueNamedPlayer(name);
            if (target == null)
            {
                SendError("Player not found!");
                return;
            }
            if (target.tradeTarget != null && target.tradeTarget != this)
            {
                SendError(target.Name + " is already trading!");
                return;
            }

            if (potentialTrader.ContainsKey(target))
            {
                tradeTarget = target;
                trade = new bool[12];
                tradeAccepted = false;
                target.tradeTarget = this;
                target.trade = new bool[12];
                target.tradeAccepted = false;
                potentialTrader.Clear();
                target.potentialTrader.Clear();

                TradeItem[] my = new TradeItem[Inventory.Length];
                for (int i = 0; i < Inventory.Length; i++)
                    my[i] = new TradeItem()
                    {
                        Item = Inventory[i] == null ? -1 : Inventory[i].ObjectType,
                        SlotType = SlotTypes[i],
                        Included = false,
                        Tradeable = (Inventory[i] == null || i < 4) ? false : !Inventory[i].Soulbound
                    };
                TradeItem[] your = new TradeItem[target.Inventory.Length];
                for (int i = 0; i < target.Inventory.Length; i++)
                    your[i] = new TradeItem()
                    {
                        Item = target.Inventory[i] == null ? -1 : target.Inventory[i].ObjectType,
                        SlotType = target.SlotTypes[i],
                        Included = false,
                        Tradeable = (target.Inventory[i] == null || i < 4) ? false : !target.Inventory[i].Soulbound
                    };

                Client.SendPacket(new TradeStartPacket()
                {
                    MyItems = my,
                    YourName = target.Name,
                    YourItems = your
                });
                target.Client.SendPacket(new TradeStartPacket()
                {
                    MyItems = your,
                    YourName = Name,
                    YourItems = my
                });
            }
            else
            {
                target.potentialTrader[this] = 1000 * 20;
                target.Client.SendPacket(new TradeRequestedPacket()
                {
                    Name = Name
                });
                SendInfo("You have sent a trade request to " + target.Name + "!");
                return;
            }
        }

        public void CancelTrade()
        {
            Client.SendPacket(new TradeDonePacket()
            {
                Result = 1,
                Message = "Trade canceled!"
            });
            tradeTarget.Client.SendPacket(new TradeDonePacket()
            {
                Result = 1,
                Message = "Trade canceled!"
            });
            ResetTrade();
        }

        public void ResetTrade()
        {
            tradeTarget.tradeTarget = null;
            tradeTarget.trade = null;
            tradeTarget.tradeAccepted = false;
            tradeTarget.Inventory.InventoryChanged -= MonitorInventory;
            tradeTarget = null;
            trade = null;
            tradeAccepted = false;
            Inventory.InventoryChanged -= MonitorInventory;
        }

        public void MonitorTrade()
        {
            Inventory.InventoryChanged += MonitorInventory;
        }

        private void MonitorInventory(object sender, InventoryChangedEventArgs e)
        {
            Player parent = (sender as Inventory).Parent as Player;
            if (parent == null) return;
            parent.CancelTrade();
        }

        private void CheckTradeTimeout(RealmTime time)
        {
            List<Tuple<Player, int>> newState = new List<Tuple<Player, int>>();
            foreach (var i in potentialTrader)
                newState.Add(new Tuple<Player, int>(i.Key, i.Value - time.thisTickTimes));

            foreach (var i in newState)
            {
                if (i.Item2 < 0)
                {
                    i.Item1.SendInfo("Trade to " + Name + " has timed out!");
                    potentialTrader.Remove(i.Item1);
                }
                else potentialTrader[i.Item1] = i.Item2;
            }
        }
    }
}
