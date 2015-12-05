using common;
using System;
using System.Collections.Generic;

namespace server.@char
{
    internal class list : RequestHandler
    {
        private Lazy<List<ServerItem>> svrList;

        public list()
        {
            svrList = new Lazy<List<ServerItem>>(GetServerList, true);
        }

        private List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            int num = Program.Settings.GetValue<int>("svrNum");
            for (int i = 0; i < Math.Min(num, 1); i++)
                ret.Add(new ServerItem()
                {
                    Name = Program.Settings.GetValue($"svr{i}Name", "Redis"),
                    Lat = Program.Settings.GetValue<int>($"svr{i}Lat", "0"),
                    Long = Program.Settings.GetValue<int>($"svr{i}Long", "0"),
                    DNS = Program.Settings.GetValue($"svr{i}Adr", "127.0.0.1"),
                    Usage = 0.2,
                    AdminOnly = Program.Settings.GetValue<bool>($"svr{i}Admin", "false")
                });
            return ret;
        }

        protected override void HandleRequest()
        {
            DbAccount acc;
            var status = Database.Verify(Query["guid"], Query["password"], out acc);
            if (status == LoginStatus.OK || status == LoginStatus.AccountNotExists)
            {
                if (status == LoginStatus.AccountNotExists)
                    acc = Database.CreateGuestAccount(Query["guid"]);
                var list = CharList.FromDb(Database, acc);
                list.Servers = GetServerList();
                WriteLine(list.ToXml().ToString());
            }
            else
                WriteErrorLine(status.GetInfo());
        }
    }
}
