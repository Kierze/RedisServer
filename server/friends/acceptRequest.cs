using common;
using System.Collections.Generic;

namespace server.friends
{
    internal class acceptRequest : RequestHandler
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
                    f.Remove(acc.AccountId);
                    target.FriendRequests = f.ToArray();
                    f = new List<string>(target.Friends);
                    f.Add(acc.AccountId);
                    target.Friends = f.ToArray();
                    target.Flush();

                    f = new List<string>(acc.Friends);
                    f.Add(target.AccountId);
                    acc.Friends = f.ToArray();
                    acc.Flush();
                    WriteLine("<Success />");
                }
                else
                    WriteErrorLine("Error.targetAccountNotFound");
            else
                WriteErrorLine("Error.accountNotFound");
        }
    }
}
