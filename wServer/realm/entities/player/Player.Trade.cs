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
            var target = Owner.GetUniqueNamedPlayer(name);

            if (target == null)
            {
                SendInfo("{\"key\":\"server.player_not_found\",\"tokens\":{\"player\":\"" + name + "\"}}");
                return;
            }
            if (!NameChosen || !target.NameChosen)
            {
                SendInfo("{\"key\":\"server.trade_needs_their_name\"}");
                return;
            }
            if (Client.Player == target)
            {
                SendInfo("{\"key\":\"server.self_trade\"}");
                return;
            }
            if (tradeTarget != null)
            {
                SendInfo("You're already trading!");
                return;
            }
            if (target.tradeTarget != null && target.tradeTarget != this)
            {
                SendInfo("{\"key\":\"server.they_already_trading\",\"tokens\":{\"player\":\"" + target.Name + "\"}}");
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
                SendInfo("{\"key\":\"server.trade_requested\",\"tokens\":{\"player\":\"" + target.Name + "\"}}");
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
            tradeTarget = null;
            trade = null;
            tradeAccepted = false;
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
                    i.Item1.SendInfo("{\"key\":\"server.trade_timeout\"}");
                    potentialTrader.Remove(i.Item1);
                }
                else potentialTrader[i.Item1] = i.Item2;
            }
        }
    }
}
