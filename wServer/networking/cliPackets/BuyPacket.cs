using common;

namespace wServer.networking.cliPackets
{
    public class BuyPacket : ClientPacket
    {
        public int ObjectId { get; set; }

        public int Quantity { get; set; }

        public override PacketID ID
        {
            get { return PacketID.BUY; }
        }

        public override Packet CreateInstance()
        {
            return new BuyPacket();
        }

        protected override void Read(NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
            Quantity = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(Quantity);
        }
    }
}
