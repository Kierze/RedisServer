using common;

namespace wServer.networking.svrPackets
{
    public class InvResultPacket : ServerPacket
    {
        public int Result { get; set; }

        public override PacketID ID { get { return PacketID.INVRESULT; } }

        public override Packet CreateInstance()
        {
            return new InvResultPacket();
        }

        protected override void Read(NReader rdr)
        {
            Result = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Result);
        }
    }
}
