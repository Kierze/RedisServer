using common;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server.@char
{
    internal class delete : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbAccount acc;
            var status = Database.Verify(query["guid"], query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                using (var l = Database.Lock(acc))
                    if (Database.LockOk(l))
                    {
                        Database.DeleteCharacter(acc, int.Parse(query["charId"]));
                        Write(context, "<Success />");
                    }
                    else
                        Write(context, "<Error>Account in Use</Error>");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
