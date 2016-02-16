using common;

namespace wServer.networking.cliPackets
{
    public class GuildRemovePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GUILDREMOVE; }
        }

        public override Packet CreateInstance()
        {
            return new GuildRemovePacket();
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
