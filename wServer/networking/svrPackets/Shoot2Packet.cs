using common;

namespace wServer.networking.svrPackets
{
    public class Shoot2Packet : ServerPacket
    {
        public byte BulletId { get; set; }

        public int OwnerId { get; set; }

        public int ContainerType { get; set; }

        public Position StartingPos { get; set; }

        public float Angle { get; set; }

        public short Damage { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOOT2; }
        }

        public override Packet CreateInstance()
        {
            return new Shoot2Packet();
        }

        protected override void Read(NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            ContainerType = rdr.ReadInt32();
            StartingPos = Position.Read(rdr);
            Angle = rdr.ReadSingle();
            Damage = rdr.ReadInt16();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(ContainerType);
            StartingPos.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
        }
    }
}
