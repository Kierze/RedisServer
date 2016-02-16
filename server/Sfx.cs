using System;
using static System.String;

namespace server
{
    internal class Sfx : RequestHandler
    {
        protected override void HandleRequest()
        {
            string file = (AppDomain.CurrentDomain.BaseDirectory + "resources/") + (Context.Request.Url.LocalPath.StartsWith("/music") ? "sfx" + Context.Request.Url.LocalPath : Context.Request.Url.LocalPath.Remove(0, 1));
            Context.Response.Redirect("http://realmofthemadgodhrd.appspot.com/" + (file.Split('/')[2].Contains("music") ? file.Replace("sfx/", Empty) : file));
        }
    }
}
