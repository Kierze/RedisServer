using System.IO;

namespace server.app
{
    internal class globalNews : RequestHandler
    {
        // none?
        protected override void HandleRequest()
        {
            WriteLine(File.ReadAllText("resources/app/globalNews.txt"), false);
        }
    }
}
