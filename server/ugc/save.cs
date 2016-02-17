using System;

namespace server.ugc
{
    internal class save : RequestHandler
    {
        // not on prod webserver
        // guid, name, description, width, height, mapjm, tags, totalObjects, totalTiles, thumbnail (?), overwrite (on/off)
        protected override void HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
