using System.Collections.Generic;
using wServer.networking.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        //TODO: persistence
        private const int LOCKED_LIST_ID = 0;

        private const int IGNORED_LIST_ID = 1;
        private List<int> locked = new List<int>(6);
        private List<int> ignored = new List<int>(6);

        private void SendAccountList(List<string> list, int id)
        {
            Client.SendPacket(new AccountListPacket()
            {
                AccountListId = id,
                AccountIds = list.ToArray()
            });
        }

        //public void EditAccountList(EditAccountListPacket pkt)
        //{
        //    List<int> list;
        //    if (pkt.AccountListId == LOCKED_LIST_ID)
        //        list = locked;
        //    else if (pkt.AccountListId == IGNORED_LIST_ID)
        //        list = ignored;
        //    else return;

        // Player player = Owner.GetEntity(pkt.ObjectId) as Player; if (player == null) return; int
        // accId = player.client.Account.AccountId;

        // if (pkt.Add && list.Count < 6) list.Add(accId); else list.Remove(accId);

        //    SendAccountList(list, pkt.AccountListId);
        //}
    }
}
