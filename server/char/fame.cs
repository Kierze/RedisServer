using common;

namespace server.@char
{
    internal class fame : RequestHandler
    {
        protected override void HandleRequest()
        {
            DbChar character = Database.LoadCharacter(int.Parse(Query["accountId"]), int.Parse(Query["charId"]));
            if (character == null)
            {
                WriteErrorLine("Invalid character");
                return;
            }

            Fame fame = Fame.FromDb(character);
            if (fame == null)
            {
                WriteErrorLine("Character not dead");
                return;
            }
            WriteLine(fame.ToXml().ToString());
        }
    }
}
