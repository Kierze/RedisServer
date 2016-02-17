using common;

namespace wServer.networking.svrPackets
{
    public class NewAbilityUnlockedPacket : ServerPacket
    {
        public int Type { get; set; }

        public override PacketID ID
        {
            get { return PacketID.NEWABILITYUNLOCKED; }
        }

        public override Packet CreateInstance()
        {
            return new NewAbilityUnlockedPacket();
        }

        protected override void Read(NReader rdr)
        {
            Type = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Type);
        }
    }
}
