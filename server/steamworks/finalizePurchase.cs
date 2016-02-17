using System;

namespace server.steamworks
{
    internal class finalizePurchase : RequestHandler
    {
        // appid, orderid, authorized (0)
        // <Error>Unable to find orderid</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
