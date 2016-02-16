using common;

namespace wServer.networking.cliPackets
{
    public class PetCommandPacket : ClientPacket
    {
        public const int FOLLOW_PET = 1;
        public const int UNFOLLOW_PET = 2;
        public const int RELEASE_PET = 3;

        public int CommandId { get; set; }

        public uint PetId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PETCOMMAND; }
        }

        public override Packet CreateInstance()
        {
            return new PetCommandPacket();
        }

        protected override void Read(NReader rdr)
        {
            CommandId = (int)rdr.ReadByte();
            PetId = (uint)rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write((byte)CommandId);
            wtr.Write((int)PetId);
        }
    }
}
