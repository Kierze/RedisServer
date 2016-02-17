using System;

namespace server.migrate
{
    internal class doMigration : RequestHandler
    {
        // guid, password
        // <Migrate><MigrateStatus>0</MigrateStatus></Migrate> -> Cant migrate ?
        // <Migrate><MigrateStatus>4</MigrateStatus></Migrate>
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
