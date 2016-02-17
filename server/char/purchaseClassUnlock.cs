using common;

namespace server.@char
{
    internal class purchaseClassUnlock : RequestHandler
    {
        // guid, password, classType
        // <Error>Bad input to character unlock</Error>
        // <Error><Text>Not enough Gold.</Text></Error>
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                Database.ChangeClassAvailability(acc, GameData, ushort.Parse(Query["classType"]));
                WriteLine("<Success />");
            }
            else
                WriteErrorLine("");
        }
    }
}
