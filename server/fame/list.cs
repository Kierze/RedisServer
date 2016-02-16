using common;

namespace server.fame
{
    internal class list : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbChar character = null;
            if (Query["accountId"] != null)
            {
                character = Database.LoadCharacter(Query["accountId"], int.Parse(Query["charId"]));
            }
            var list = FameList.FromDb(Database, Query["timespan"], character);
            WriteLine(list.ToXml());
        }
    }
}
