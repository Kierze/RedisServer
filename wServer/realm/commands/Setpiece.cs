using System;
using wServer.realm.entities;
using wServer.realm.setpieces;

namespace wServer.realm.commands
{
    internal class CommandSetpiece : Command
    {
        public CommandSetpiece() : base("setpiece", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                "wServer.realm.setpieces." + args, true, true));
            piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
            return true;
        }
    }
}
