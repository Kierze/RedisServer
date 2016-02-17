using common;

namespace wServer.networking.svrPackets
{
    public class ClientStatPacket : ServerPacket
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CLIENTSTAT; }
        }

        public override Packet CreateInstance()
        {
            return new ClientStatPacket();
        }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
            Value = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Value);
        }
    }
}
