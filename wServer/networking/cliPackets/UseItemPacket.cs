using common;

namespace wServer.networking.cliPackets
{
    public class UseItemPacket : ClientPacket
    {
        public int Time { get; set; }

        public ObjectSlot SlotObject { get; set; }

        public Position ItemUsePos { get; set; }

        public byte UseType { get; set; }

        public override PacketID ID
        {
            get { return PacketID.USEITEM; }
        }

        public override Packet CreateInstance()
        {
            return new UseItemPacket();
        }

        protected override void Read(NReader rdr)
        {
            Time = rdr.ReadInt32();
            SlotObject = ObjectSlot.Read(rdr);
            ItemUsePos = Position.Read(rdr);
            UseType = rdr.ReadByte();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Time);
            SlotObject.Write(wtr);
            ItemUsePos.Write(wtr);
            wtr.Write(UseType);
        }
    }
}
