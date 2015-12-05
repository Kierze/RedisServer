using common;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server
{
    public abstract class RequestHandler
    {
        protected NameValueCollection Query { get; private set; }
        protected HttpListenerContext Context { get; private set; }
        protected Database Database => Program.Database;

        public void HandleRequest(HttpListenerContext context)
        {
            this.Context = context;
            if (ParseQueryString())
            {
                Query = new NameValueCollection();
                using (var reader = new StreamReader(context.Request.InputStream))
                    Query = HttpUtility.ParseQueryString(reader.ReadToEnd());

                if (Query.AllKeys.Length == 0)
                {
                    string currurl = context.Request.RawUrl;
                    int iqs = currurl.IndexOf('?');
                    if (iqs >= 0)
                        Query = HttpUtility.ParseQueryString((iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : string.Empty);
                }
            }

            HandleRequest();
        }

        public void WriteLine(string value, params object[] args)
        {
            using (var writer = new StreamWriter(Context.Response.OutputStream))
                if (args == null || args.Length == 0) writer.Write(value);
                else writer.Write(value, args);
        }

        public void WriteErrorLine(string value, params object[] args)
        {
            using (var writer = new StreamWriter(Context.Response.OutputStream))
                writer.Write("<Error>" + value + "</Error>", args);
        }

        protected virtual bool ParseQueryString() => true;

        protected abstract void HandleRequest();
    }
}
