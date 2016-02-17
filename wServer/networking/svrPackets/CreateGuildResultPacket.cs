using common;

namespace wServer.networking.svrPackets
{
    public class CreateGuildResultPacket : ServerPacket
    {
        public bool Success { get; set; }

        public string ErrorText { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CREATEGUILDRESULT; }
        }

        public override Packet CreateInstance()
        {
            return new CreateGuildResultPacket();
        }

        protected override void Read(NReader rdr)
        {
            Success = rdr.ReadBoolean();
            ErrorText = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
