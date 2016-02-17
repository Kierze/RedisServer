using common;

namespace wServer.networking.svrPackets
{
    public class RemovePetFromListPacket : ServerPacket
    {
        public int PetId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.REMOVEPET; }
        }

        public override Packet CreateInstance()
        {
            return new RemovePetFromListPacket();
        }

        protected override void Read(NReader rdr)
        {
            PetId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(PetId);
        }
    }
}
