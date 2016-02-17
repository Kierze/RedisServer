using System.Collections.Generic;
using wServer.networking.svrPackets;
using wServer.realm.worlds;

namespace wServer.realm.entities
{
    public class SellableObject : StaticObject
    {
        private const int BUY_NO_GOLD = 3;
        private const int BUY_NO_FAME = 6;

        public SellableObject(RealmManager manager, ushort objType)
            : base(manager, objType, null, true, false, false)
        {
            if (objType == 0x0505)  //Vault chest
            {
                Price = 100;
                Currency = CurrencyType.Gold;
                RankReq = 0;
            }
        }

        public int Price { get; set; }

        public CurrencyType Currency { get; set; }

        public int RankReq { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellablePriceCurrency] = (int)Currency;
            stats[StatsType.SellableRankRequirement] = RankReq;
            base.ExportStats(stats);
        }

        protected virtual bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            if (!player.NameChosen) return false;
            if (player.Stars < RankReq) return false;

            if (Currency == CurrencyType.Fame)
                if (acc.Fame < Price)
                    return false;

            if (Currency == CurrencyType.Gold)
                if (acc.Credits < Price)
                    return false;
            return true;
        }

        public virtual void Buy(Player player)
        {
            if (ObjectType == 0x0505)   //Vault chest
            {
                if (TryDeduct(player))
                {
                    (Owner as Vault).AddChest(this);
                    player.Client.SendPacket(new BuyResultPacket()
                    {
                        Result = 0,
                        Message = "{\"key\":\"server.buy_success\"}"
                    });
                }
                else
                    player.Client.SendPacket(new BuyResultPacket()
                    {
                        Result = BUY_NO_GOLD,
                        Message = "{\"key\":\"server.not_enough_gold\"}"
                    });
            }
        }
    }
}
