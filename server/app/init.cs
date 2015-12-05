using System.IO;

namespace server.app
{
    internal class init : RequestHandler
    {
        protected override void HandleRequest()
        {
            WriteLine(File.ReadAllText("text/init.txt"));
        }
    }
}
