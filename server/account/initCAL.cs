using common;

namespace server.account
{
    internal class initCAL : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                if (acc.Admin)
                {
                    var ca = new DbClassAvailability(acc);
                    ca.Init(GameData);
                    ca.Flush();
                    WriteLine("<Success />");
                }
                else
                    WriteLine("<Failure />");
            }
            else
            {
                WriteErrorLine("<Failure />");
            }
        }
    }
}
