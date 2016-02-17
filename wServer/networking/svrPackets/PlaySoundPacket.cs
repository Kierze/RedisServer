using common;

namespace wServer.networking.svrPackets
{
    public class PlaySoundPacket : ServerPacket
    {
        public int OwnerId { get; set; }

        public int SoundId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PLAYSOUND; }
        }

        public override Packet CreateInstance()
        {
            return new PlaySoundPacket();
        }

        protected override void Read(NReader rdr)
        {
            OwnerId = rdr.ReadInt32();
            SoundId = rdr.ReadByte();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(OwnerId);
            wtr.Write((byte)SoundId);
        }
    }
}
