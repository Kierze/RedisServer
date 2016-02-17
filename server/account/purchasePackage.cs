using System;

namespace server.account
{
    internal class purchasePackage : RequestHandler
    {
        // guid, password,
        // <Error>Invalid PackageId</Error>
        // <Error>This package is not available any more</Error>
        // <Error>Not enough Gold to purchase</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
