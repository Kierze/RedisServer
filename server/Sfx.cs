using System;

namespace server
{
    internal class Sfx : RequestHandler
    {
        protected override void HandleRequest()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + (Context.Request.Url.LocalPath.StartsWith("/music") ? "sfx" + Context.Request.Url.LocalPath : Context.Request.Url.LocalPath.Remove(0, 1));
            Context.Response.Redirect("http://realmofthemadgodhrd.appspot.com/" + (file.Split('/')[1].Contains("music") ? file.Replace("sfx/", String.Empty) : file));
        }
    }
}
