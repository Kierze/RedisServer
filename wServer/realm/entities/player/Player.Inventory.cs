using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private readonly Random invRand = new Random();

        public void DropBag(Item i)
        {
            ushort bagId = 0x0500;
            var soulbound = false;
            if (i.Soulbound)
            {
                bagId = 0x0503;
                soulbound = true;
            }

            var container = new Container(Manager, bagId, 1000 * 60, true);
            if (soulbound)
                container.BagOwners = new[] { AccountId };
            container.Inventory[0] = i;
            container.Move(X + (float)((invRand.NextDouble() * 2 - 1) * 0.5),
                Y + (float)((invRand.NextDouble() * 2 - 1) * 0.5));
            container.Size = 75;
            Owner.EnterWorld(container);
        }

    }
}
