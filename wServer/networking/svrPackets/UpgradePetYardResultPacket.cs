using common;

namespace wServer.networking.svrPackets
{
    public class UpgradePetYardResultPacket : ServerPacket
    {
        public int Type { get; set; }

        public override PacketID ID
        {
            get { return PacketID.UPGRADEPETYARDRESULT; }
        }

        public override Packet CreateInstance()
        {
            return new UpgradePetYardResultPacket();
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
