using common;

namespace wServer.networking.cliPackets
{
    public class EscapePacket : ClientPacket
    {
        public override PacketID ID
        {
            get { return PacketID.ESCAPE; }
        }

        public override Packet CreateInstance()
        {
            return new EscapePacket();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
