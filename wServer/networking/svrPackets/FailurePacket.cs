using common;

namespace wServer.networking.svrPackets
{
    public class FailurePacket : ServerPacket
    {
        public int ErrorId { get; set; }

        public string ErrorDescription { get; set; }

        public override PacketID ID
        {
            get { return PacketID.FAILURE; }
        }

        public override Packet CreateInstance()
        {
            return new FailurePacket();
        }

        protected override void Read(NReader rdr)
        {
            ErrorId = rdr.ReadInt32();
            ErrorDescription = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ErrorId);
            wtr.WriteUTF(ErrorDescription);
        }
    }
}
