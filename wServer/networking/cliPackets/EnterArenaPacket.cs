using common;

namespace wServer.networking.cliPackets
{
    public class EnterArenaPacket : ClientPacket
    {
        public int Currency { get; set; }

        public override PacketID ID
        {
            get { return PacketID.ENTER_ARENA; }
        }

        public override Packet CreateInstance()
        {
            return new EnterArenaPacket();
        }

        protected override void Read(NReader rdr)
        {
            Currency = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Currency);
        }
    }
}
