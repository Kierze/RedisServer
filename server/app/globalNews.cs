using System.IO;

namespace server.app
{
    internal class globalNews : RequestHandler
    {
        protected override void HandleRequest()
        {
            WriteLine(File.ReadAllText("text/news.txt"));
        }
    }
}
