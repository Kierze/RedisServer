using common;

namespace server.friends
{
    internal class getList : RequestHandler
    {
        // guid, password, ignore
        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK)
                WriteLine(FriendList.FromDb(Database, acc).ToXml());
            else
                WriteLine("<Friends></Friends>");
        }
    }
}

/**
<?xml version="1.0" encoding="UTF-8"?>
<Friends>
    <Account>
        <Character>
            <ObjectType>797</ObjectType>
            <Level>20</Level>
            <Exp>20460</Exp>
            <CurrentFame>20</CurrentFame>
            <Texture>29790</Texture>
        </Character>
        <Name>Mark</Name>
        <Stats>
            <Stats>
                <ClassStats objectType="0x300">
                    <BestLevel>20</BestLevel>
                    <BestFame>8317</BestFame>
                </ClassStats>
                <ClassStats objectType="0x307">
                    <BestLevel>20</BestLevel>
                    <BestFame>10059</BestFame>
                </ClassStats>
                <ClassStats objectType="0x30e">
                    <BestLevel>20</BestLevel>
                    <BestFame>3696</BestFame>
                </ClassStats>
                <ClassStats objectType="0x310">
                    <BestLevel>20</BestLevel>
                    <BestFame>4136</BestFame>
                </ClassStats>
                <ClassStats objectType="0x31d">
                    <BestLevel>20</BestLevel>
                    <BestFame>2059</BestFame>
                </ClassStats>
                <ClassStats objectType="0x31e">
                    <BestLevel>20</BestLevel>
                    <BestFame>10000</BestFame>
                </ClassStats>
                <ClassStats objectType="0x31f">
                    <BestLevel>20</BestLevel>
                    <BestFame>5033</BestFame>
                </ClassStats>
                <ClassStats objectType="0x320">
                    <BestLevel>20</BestLevel>
                    <BestFame>10000</BestFame>
                </ClassStats>
                <ClassStats objectType="0x321">
                    <BestLevel>20</BestLevel>
                    <BestFame>3164</BestFame>
                </ClassStats>
                <ClassStats objectType="0x322">
                    <BestLevel>20</BestLevel>
                    <BestFame>2014</BestFame>
                </ClassStats>
                <ClassStats objectType="0x323">
                    <BestLevel>20</BestLevel>
                    <BestFame>2027</BestFame>
                </ClassStats>
                <ClassStats objectType="0x324">
                    <BestLevel>20</BestLevel>
                    <BestFame>2010</BestFame>
                </ClassStats>
                <ClassStats objectType="0x325">
                    <BestLevel>20</BestLevel>
                    <BestFame>16135</BestFame>
                </ClassStats>
                <ClassStats objectType="0x326">
                    <BestLevel>20</BestLevel>
                    <BestFame>2713</BestFame>
                </ClassStats>
                <BestCharFame>16135</BestCharFame>
                <TotalFame>183234</TotalFame>
                <Fame>19220</Fame>
            </Stats>
        </Stats>
    </Account>
</Friends>
*/
