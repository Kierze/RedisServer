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
                character = Database.LoadCharacter(
                    int.Parse(Query["accountId"]),
                    int.Parse(Query["charId"])
                );
            }
            FameList list = FameList.FromDb(Database, Query["timespan"], character);
            WriteLine(list.ToXml().ToString());
        }
    }
}
