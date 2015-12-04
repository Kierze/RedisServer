using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandTutorial : Command
    {
        public CommandTutorial() : base("tutorial")
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array,
            });
            return true;
        }
    }
}
