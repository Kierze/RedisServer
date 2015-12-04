using System.Net;

namespace server.account
{
    internal class forgotPassword : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            Write(context, "<Error>Nope.</Error>");
        }
    }
}
