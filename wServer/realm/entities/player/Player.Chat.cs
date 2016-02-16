using wServer.networking.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        public void SendInfo(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "",
                Text = text
            });
        }

        public void SendError(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Error*",
                Text = text
            });
        }

        public void SendClientText(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Client*",
                Text = text
            });
        }

        public void SendHelp(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Help*",
                Text = text
            });
        }

        public void SendEnemy(string name, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#" + name,
                Text = text
            });
        }

        public void SendText(string sender, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = sender,
                Text = text
            });
        }

        internal void TellReceived(int objId, int stars, string from, string to, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 10,
                Stars = stars,
                Name = from,
                Recipient = to,
                Text = text
            });
        }

        internal void AnnouncementReceived(string text)
        {
            Client.Player.SendText("@Announcement", text);
        }

        internal void GuildReceived(int objId, int stars, string from, string text)
        {
            //untested
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 10,
                Stars = stars,
                Name = "*Guild*",
                Recipient = from,
                Text = text
            });
        }
    }
}
