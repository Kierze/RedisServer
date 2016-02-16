using common;

namespace wServer.networking.cliPackets
{
    public class TinkerQuestPacket : ClientPacket
    {
        public ObjectSlot Object { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TINKERQUEST; }
        }

        public override Packet CreateInstance()
        {
            return new TinkerQuestPacket();
        }

        protected override void Read(NReader rdr)
        {
            Object = ObjectSlot.Read(rdr);
        }

        protected override void Write(NWriter wtr)
        {
            Object.Write(wtr);
        }
    }
}
