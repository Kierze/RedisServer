using common;

namespace wServer.networking.svrPackets
{
    public class PicPacket : ServerPacket
    {
        public BitmapData BitmapData { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PIC; }
        }

        public override Packet CreateInstance()
        {
            return new PicPacket();
        }

        protected override void Read(NReader rdr)
        {
            BitmapData = BitmapData.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            BitmapData.Write(wtr);
        }
    }
}
