using System;
using common;
using wServer.realm.entities;
using wServer.networking.unknownPackets;

namespace wServer.realm.commands
{
    internal class TestCommand : Command
    {
        public TestCommand() : base("test") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.SendPacket(new PickNewPetPacket
            {
                
            });
            return true;
        }
    }

    internal class DebugCommand : Command
    {
        public DebugCommand() : base("debug", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Owner.EnterWorld(new Locater(player));
            return true;
        }

        private class Locater : Enemy
        {
            private Player player;

            public Locater(Player player) : base(player.Manager, 0x0d5d)
            {
                this.player = player;
                Move(player.X, player.Y);
                ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                });
            }

            public override void Tick(RealmTime time)
            {
                Move(player.X, player.Y);
                UpdateCount++;
                base.Tick(time);
            }
        }
    }
}
