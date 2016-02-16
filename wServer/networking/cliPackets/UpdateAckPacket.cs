using common;

namespace wServer.networking.cliPackets
{
    public class UpdateAckPacket : ClientPacket
    {
        public override PacketID ID
        {
            get { return PacketID.UPDATEACK; }
        }

        public override Packet CreateInstance()
        {
            return new UpdateAckPacket();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
