using common;

namespace wServer.networking.svrPackets
{
    public class InvitedToGuildPacket : ServerPacket
    {
        public string Name { get; set; }

        public string GuildName { get; set; }

        public override PacketID ID
        {
            get { return PacketID.INVITEDTOGUILD; }
        }

        public override Packet CreateInstance()
        {
            return new InvitedToGuildPacket();
        }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
            GuildName = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(GuildName);
        }
    }
}
