using common;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace server.account
{
    internal class register : RequestHandler
    {
        public bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            MatchEvaluator DomainMapper = match =>
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            };

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
        }

        protected override void HandleRequest()
        {
            if (!IsValidEmail(Query["newGUID"]))
                WriteErrorLine("Invalid email");
            else
            {
                string key = Database.REG_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = Database.AcquireLock(key)) == null) ;

                    DbAccount acc;
                    var status = Database.Verify(Query["guid"], "", out acc);
                    if (status == LoginStatus.OK)
                    {
                        //what? can register in game? kill the account lock
                        Database.RenameUUID(acc, Query["newGUID"], lockToken);
                        Database.ChangePassword(acc.UUID, Query["newPassword"]);
                        WriteLine("<Success />");
                    }
                    else
                    {
                        var s = Database.Register(Query["newGUID"], Query["newPassword"], false, out acc);
                        if (s == RegisterStatus.OK)
                            WriteLine("<Success />");
                        else
                            WriteErrorLine(s.GetInfo());
                    }
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
