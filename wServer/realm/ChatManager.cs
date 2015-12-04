using common;
using log4net;
using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.realm
{
    public class ChatManager
    {
        private const char TELL = 't';
        private const char GUILD = 'g';
        private const char ANNOUNCE = 'a';

        private struct Message
        {
            public char Type;
            public string Inst;

            public int ObjId;
            public int Stars;
            public int From;

            public int To;
            public string Text;
        }

        private static ILog log = LogManager.GetLogger(typeof(ChatManager));

        private RealmManager manager;

        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
            manager.InterServer.AddHandler<Message>(ISManager.CHAT, HandleChat);
        }

        public void Say(Player src, string text)
        {
            src.Owner.BroadcastPacket(new TextPacket()
            {
                Name = (src.Client.Account.Admin ? "@" : "") + src.Name,
                ObjectId = src.Id,
                Stars = src.Stars,
                BubbleTime = 5,
                Recipient = "",
                Text = text,
                CleanText = text
            }, null);
            log.Info($"[{src.Owner.Name} ({src.Owner.Id})] <{src.Name}> {text}");
        }

        public void Announce(string text)
        {
            manager.InterServer.Publish(ISManager.CHAT, new Message()
            {
                Type = ANNOUNCE,
                Inst = manager.InstanceId,
                Text = text
            });
        }

        public void Oryx(World world, string text)
        {
            world.BroadcastPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#Oryx the Mad God",
                Text = text
            }, null);
            log.Info($"[{world.Name} ({world.Id})] <Oryx the Mad God> {text}");
        }

        public bool Tell(Player src, string target, string text)
        {
            int id = manager.Database.ResolveId(target);
            if (id == 0) return false;

            int time = manager.Database.GetLockTime(id);
            if (time == -1) return false;

            manager.InterServer.Publish(ISManager.CHAT, new Message()
            {
                Type = TELL,
                Inst = manager.InstanceId,
                ObjId = src.Id,
                Stars = src.Stars,
                From = src.Client.Account.AccountId,
                To = id,
                Text = text
            });
            return true;
        }

        private void HandleChat(object sender, InterServerEventArgs<Message> e)
        {
            switch (e.Content.Type)
            {
                case TELL:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        string to = manager.Database.ResolveIgn(e.Content.To);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.AccountId == e.Content.From ||
                                        x.Account.AccountId == e.Content.To)
                            .Select(x => x.Player))
                        {
                            i.TellReceived(
                                e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                                e.Content.Stars, from, to, e.Content.Text);
                        }
                    }
                    break;
                case GUILD:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.GuildId == e.Content.To)
                            .Select(x => x.Player))
                        {
                            i.GuildReceived(
                                e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                                e.Content.Stars, from, e.Content.Text);
                        }
                    }
                    break;
                case ANNOUNCE:
                    {
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Select(x => x.Player))
                        {
                            i.AnnouncementReceived(e.Content.Text);
                        }
                    }
                    break;
            }
        }
    }
}
