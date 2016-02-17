using common;
using System.Collections.Generic;

namespace server.friends
{
    internal class requestFriend : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbAccount acc;
            DbAccount target;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
                if ((target = Database.GetAccountById(Database.ResolveId(Query["targetName"] ?? ""))) != null)
                {
                    List<string> f = new List<string>(target.FriendRequests);
                    f.Add(acc.AccountId);
                    target.FriendRequests = f.ToArray();
                    target.Flush();
                    WriteLine("<Success />");
                }
                else
                    WriteErrorLine("Error.targetAccountNotFound");
            else
                WriteErrorLine("Error.accountNotFound");
        }
    }
}
