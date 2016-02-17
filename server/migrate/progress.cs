using System;

namespace server.migrate
{
    internal class progress : RequestHandler
    {
        // <Error>need guid as param</Error>
        // <Migrate><MigrateStatus>44</MigrateStatus></Migrate>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
