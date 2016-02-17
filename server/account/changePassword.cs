using common;

namespace server.account
{
    internal class changePassword : RequestHandler
    {
        // guid, password, newPassword
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                Database.ChangePassword(Query["guid"], Query["newPassword"]);
                WriteLine("<Success />");
            }
            else
                WriteErrorLine(status.GetInfo());
        }
    }
}
