using common;

namespace server.account
{
    internal class getBeginnerPackageTimeLeft : RequestHandler
    {
        // guid, password
        // default -> <BeginnerPackageTimeLeft>604800</BeginnerPackageTimeLeft>
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
                WriteLine($"<BeginnerPackageTimeLeft>10</BeginnerPackageTimeLeft>");
            else
                WriteLine("<BeginnerPackageTimeLeft>604800</BeginnerPackageTimeLeft>");
        }
    }
}
