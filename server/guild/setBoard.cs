using System;

namespace server.guild
{
    internal class setBoard : RequestHandler
    {
        // guid, password, (parameter for board text)
        // <FatalError>Not a valid account</FatalError>
        // <FatalError>Not in a guild</FatalError>
        // <Error>Not authorized to change board</Error>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
