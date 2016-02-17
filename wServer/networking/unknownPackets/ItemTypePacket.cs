using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using wServer.networking.cliPackets;

namespace wServer.networking.unknownPackets
{
    class ItemTypePacket : ClientPacket
    {
        public int ItemType { get; set; }

        public override PacketID ID => PacketID.ITEMTYPE;

        public override Packet CreateInstance()
        {
            return new ItemTypePacket();
        }

        protected override void Read(NReader rdr)
        {
            ItemType = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ItemType);
        }
    }
}
