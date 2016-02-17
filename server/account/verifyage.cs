using common;

namespace server.account
{
    internal class verifyage : RequestHandler
    {
        /**
         guid
         isAgeVerified
         password
        */

        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                Database.VerifyAge(acc);
                WriteLine("<Success />");
            }
            else
            {
                WriteErrorLine("");
            }
        }
    }
}
