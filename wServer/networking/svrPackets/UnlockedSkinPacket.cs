using common;

namespace wServer.networking.svrPackets
{
    public class UnlockedSkinPacket : ServerPacket
    {
        public int SkinID { get; set; }

        public override PacketID ID
        {
            get { return PacketID.RESKIN2; }
        }

        public override Packet CreateInstance()
        {
            return new UnlockedSkinPacket();
        }

        protected override void Read(NReader rdr)
        {
            SkinID = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(SkinID);
        }
    }
}
