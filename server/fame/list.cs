using common;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server.fame
{
    internal class list : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbChar character = null;
            if (query["accountId"] != null)
            {
                character = Database.LoadCharacter(
                    int.Parse(query["accountId"]),
                    int.Parse(query["charId"])
                );
            }
            FameList list = FameList.FromDb(Database, query["timespan"], character);
            Write(context, list.ToXml().ToString());
        }
    }
}
