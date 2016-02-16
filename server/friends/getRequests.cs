using common;
using System;

namespace server.friends
{
    internal class getRequests : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
                WriteLine(FriendRequests.FromDb(Database, acc).ToXml());
            else
                WriteLine("<Requests></Requests>");
        }
    }
}
