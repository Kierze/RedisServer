using common;

namespace server.account
{
    internal class purchaseCharSlot : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                using (var l = Database.Lock(acc))
                {
                    if (!Database.LockOk(l))
                    {
                        WriteErrorLine("Account in use");
                        return;
                    }
                    else if (acc.Credits < 100)
                    {
                        WriteErrorLine("Not enough credits");
                        return;
                    }
                    Database.UpdateCredit(acc, -100);
                    acc.MaxCharSlot++;
                    acc.Flush();
                }
                WriteLine("<Success />");
            }
            else
                WriteErrorLine(status.GetInfo());
        }
    }
}
