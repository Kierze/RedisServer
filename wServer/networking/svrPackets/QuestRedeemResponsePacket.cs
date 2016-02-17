using common;

namespace wServer.networking.svrPackets
{
    public class QuestRedeemResponsePacket : ServerPacket
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public override PacketID ID
        {
            get { return PacketID.QUESTREDEEMRESPONSE; }
        }

        public override Packet CreateInstance()
        {
            return new QuestRedeemResponsePacket();
        }

        protected override void Read(NReader rdr)
        {
            Success = rdr.ReadBoolean();
            Message = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(Message);
        }
    }
}
