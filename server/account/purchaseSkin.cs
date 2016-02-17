using System;

namespace server.account
{
    internal class purchaseSkin : RequestHandler
    {
        // guid, password
        // <Error>Invalid skin parameter.</Error>
        // <Error>Error.skinTypeIsNotAnItem</Error>
        // <Error>Error.skinNotPurcasable</Error>
        // <Error>Not enough Gold.</Error>
        // <Error>Error.alreadyOwnsSkin</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
