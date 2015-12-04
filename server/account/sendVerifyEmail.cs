using System.Net;

namespace server.account
{
    internal class sendVerifyEmail : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            Write(context, "<Error>Nope.</Error>");
        }
    }
}
