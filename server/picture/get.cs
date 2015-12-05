using System.IO;
using System.Text;

namespace server.picture
{
    internal class get : RequestHandler
    {
        private byte[] buff = new byte[0x10000];

        protected override void HandleRequest()
        {
            //warning: maybe has hidden url injection
            string id = Query["id"];
            foreach (var i in id)
            {
                if (char.IsLetter(i) || i == '_' || i == '-') continue;

                byte[] status = Encoding.UTF8.GetBytes("<Error>Invalid ID.</Error>");
                Context.Response.OutputStream.Write(status, 0, status.Length);
                return;
            }

            string path = Path.GetFullPath("texture/_" + id + ".png");
            if (!File.Exists(path))
            {
                byte[] status = Encoding.UTF8.GetBytes("<Error>Invalid ID.</Error>");
                Context.Response.OutputStream.Write(status, 0, status.Length);
                return;
            }

            Context.Response.ContentType = "image/png";
            using (var i = File.OpenRead(path))
            {
                int c;
                while ((c = i.Read(buff, 0, buff.Length)) > 0)
                    Context.Response.OutputStream.Write(buff, 0, c);
            }
        }
    }
}
