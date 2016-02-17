using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using wServer.networking.svrPackets;
using wServer.networking.cliPackets;

namespace wServer.networking.unknownPackets
{
    class PickNewPetPacket : ClientPacket
    {
        public int PetInstanceId { get; set; }

        public int PickedNewPetType { get; set; }

        public override PacketID ID => PacketID.UNKNOWN;

        public override Packet CreateInstance()
        {
            return new PickNewPetPacket();
        }

        protected override void Read(NReader rdr)
        {
            PetInstanceId = rdr.ReadInt32();
            PickedNewPetType = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(PetInstanceId);
            wtr.Write(PickedNewPetType);
        }
    }
}
