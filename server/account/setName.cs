using common;
using System.Linq;

namespace server.account
{
    internal class setName : RequestHandler
    {
        protected override void HandleRequest()
        {
            string name = Query["name"];
            if (name.Length < 3 || name.Length > 15 || !name.All(x => char.IsLetter(x) || char.IsNumber(x)))
                WriteErrorLine("Invalid name");
            else
            {
                string key = Database.NAME_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = Database.AcquireLock(key)) == null) ;

                    if (Database.Hashes.Exists(0, "names", name.ToUpperInvariant()).Exec())
                    {
                        WriteErrorLine("Duplicated name");
                        return;
                    }

                    DbAccount acc;
                    var status = Database.Verify(Query["guid"], Query["password"], out acc);
                    if (status == LoginStatus.OK)
                    {
                        using (var l = Database.Lock(acc))
                            if (Database.LockOk(l))
                            {
                                if (acc.NameChosen && acc.Credits < 1000)
                                    WriteErrorLine("Not enough credits");
                                else
                                {
                                    if (acc.NameChosen)
                                        Database.UpdateCredit(acc, -1000);
                                    while (!Database.RenameIGN(acc, name, lockToken)) ;
                                    WriteLine("<Success />");
                                }
                            }
                            else
                                WriteErrorLine("Account in use");
                    }
                    else
                        WriteErrorLine(status.GetInfo());
                }
                finally
                {
                    if (lockToken != null)
                        Database.ReleaseLock(key, lockToken);
                }
            }
        }
    }
}
