using common;

namespace wServer.networking.svrPackets
{
    public class QuestObjIdPacket : ServerPacket
    {
        public int ObjectId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.QUESTOBJID; }
        }

        public override Packet CreateInstance()
        {
            return new QuestObjIdPacket();
        }

        protected override void Read(NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
