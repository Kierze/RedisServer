using common;

namespace wServer.networking.svrPackets
{
    public class FailurePacket : ServerPacket
    {
        public string Message { get; set; }

        public override PacketID ID { get { return PacketID.FAILURE; } }

        public override Packet CreateInstance()
        {
            return new FailurePacket();
        }

        protected override void Read(NReader rdr)
        {
            Message = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Message);
        }
    }
}
