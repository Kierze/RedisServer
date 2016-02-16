using common;

namespace server.account
{
    internal class getCredits : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
                WriteLine($"<Credits>{acc.Credits}</Credits>");
            else
                WriteLine("<Credits>40</Credits>");
        }
    }
}
