using common;

namespace wServer.networking.cliPackets
{
    public class GuildInvitePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GUILDINVITE; }
        }

        public override Packet CreateInstance()
        {
            return new GuildInvitePacket();
        }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
