using System;

namespace server.arena
{
    internal class getRecords : RequestHandler
    {
        // guid, password, type
        // <Error>Bad arena records request type.</Error>
        // default -> server error lol
        // default personal -> <ArenaRecords></ArenaRecords>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
