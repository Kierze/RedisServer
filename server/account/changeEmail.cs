using System;

namespace server.account
{
    internal class changeEmail : RequestHandler
    {
        // guid, password, newGuid (email)
        // no email -> <Error>Error.invalidEmail</Error>
        // email already used -> <Error>Error.emailAlreadyVerified</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
