using System;

namespace server.migrate
{
    internal class userAccountReset : RequestHandler
    {
        // not on prod webserver
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
