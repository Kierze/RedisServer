using System;

namespace server.account
{
    internal class playFortuneGame : RequestHandler
    {
        // guid, password, currency, gameId, status, choice
        // no gameId -> <Error>Invalid Game Id</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
