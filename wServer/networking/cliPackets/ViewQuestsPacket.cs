using common;

namespace wServer.networking.cliPackets
{
    public class ViewQuestsPacket : ClientPacket
    {
        public override PacketID ID
        {
            get { return PacketID.VIEWQUESTS; }
        }

        public override Packet CreateInstance()
        {
            return new ViewQuestsPacket();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
