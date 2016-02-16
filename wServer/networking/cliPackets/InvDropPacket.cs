using common;

namespace wServer.networking.cliPackets
{
    public class InvDropPacket : ClientPacket
    {
        public ObjectSlot SlotObject { get; set; }

        public override PacketID ID
        {
            get { return PacketID.INVDROP; }
        }

        public override Packet CreateInstance()
        {
            return new InvDropPacket();
        }

        protected override void Read(NReader rdr)
        {
            SlotObject = ObjectSlot.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            SlotObject.Write(wtr);
        }
    }
}
