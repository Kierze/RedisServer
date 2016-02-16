using System;

namespace server.clientError
{
    internal class add : RequestHandler
    {
        /*
        this.client.sendRequest("/clientError/add", {
                text:_local1.join("\n"),
                guid:this.account.getUserId()
            });
        */

        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
