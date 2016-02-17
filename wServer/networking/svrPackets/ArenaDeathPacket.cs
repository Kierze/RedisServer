using common;

namespace wServer.networking.svrPackets
{
    public class ArenaDeathPacket : ServerPacket
    {
        public int RestartPrice { get; set; }

        public override PacketID ID
        {
            get { return PacketID.ARENADEATH; }
        }

        public override Packet CreateInstance()
        {
            return new ArenaDeathPacket();
        }

        protected override void Read(NReader rdr)
        {
            RestartPrice = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(RestartPrice);
        }
    }
}
