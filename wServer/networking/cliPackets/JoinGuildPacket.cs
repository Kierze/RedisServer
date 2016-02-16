using common;

namespace wServer.networking.cliPackets
{
    public class JoinGuildPacket : ClientPacket
    {
        public string GuildName { get; set; }

        public override PacketID ID
        {
            get { return PacketID.JOINGUILD; }
        }

        public override Packet CreateInstance()
        {
            return new JoinGuildPacket();
        }

        protected override void Read(NReader rdr)
        {
            GuildName = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(GuildName);
        }
    }
}
