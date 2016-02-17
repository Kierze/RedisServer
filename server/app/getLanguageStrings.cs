using System.IO;

namespace server.app
{
    internal class getLanguageStrings : RequestHandler
    {
        // languageType
        // <Error>Invalid langauge type.</Error>
        protected override void HandleRequest()
        {
            WriteLine(File.ReadAllText("resources/app/languageStrings.txt"), false);
        }
    }
}
