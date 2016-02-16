using common;

namespace wServer.networking.cliPackets
{
    public class InvSwapPacket : ClientPacket
    {
        public int Time { get; set; }

        public Position Position { get; set; }

        public ObjectSlot SlotObject1 { get; set; }

        public ObjectSlot SlotObject2 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.INVSWAP; }
        }

        public override Packet CreateInstance()
        {
            return new InvSwapPacket();
        }

        protected override void Read(NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(rdr);
            SlotObject1 = ObjectSlot.Read(rdr);
            SlotObject2 = ObjectSlot.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
            SlotObject1.Write(wtr);
            SlotObject2.Write(wtr);
        }
    }
}
