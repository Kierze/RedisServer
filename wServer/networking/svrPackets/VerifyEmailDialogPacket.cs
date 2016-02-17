using common;

namespace wServer.networking.svrPackets
{
    public class VerifyEmailDialogPacket : ServerPacket
    {
        public override PacketID ID
        {
            get { return PacketID.VERIFYEMAILDIALOG; }
        }

        public override Packet CreateInstance()
        {
            return new VerifyEmailDialogPacket();
        }

        protected override void Read(NReader rdr)
        {
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
