using common;

namespace wServer.networking.svrPackets
{
    public class TradeChangedPacket : ServerPacket
    {
        public bool[] Offers { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TRADECHANGED; }
        }

        public override Packet CreateInstance()
        {
            return new TradeChangedPacket();
        }

        protected override void Read(NReader rdr)
        {
            Offers = new bool[rdr.ReadInt16()];
            for (int i = 0; i < Offers.Length; i++)
                Offers[i] = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((ushort)Offers.Length);
            foreach (bool i in Offers)
                wtr.Write(i);
        }
    }
}
