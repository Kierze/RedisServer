using common;

namespace wServer.networking.cliPackets
{
    public class SetConditionPacket : ClientPacket
    {
        public int ConditionEffect { get; set; }

        public float ConditionDuration { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SETCONDITION; }
        }

        public override Packet CreateInstance()
        {
            return new SetConditionPacket();
        }

        protected override void Read(NReader rdr)
        {
            ConditionEffect = rdr.ReadInt32();
            ConditionDuration = rdr.ReadSingle();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ConditionEffect);
            wtr.Write(ConditionDuration);
        }
    }
}
