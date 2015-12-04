using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandSpawn : Command
    {
        public CommandSpawn() : base("spawn", permLevel: 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int index = args.IndexOf(' ');
            int num;
            string name = args;

            if (args.IndexOf(' ') > 0 && int.TryParse(args.Substring(0, args.IndexOf(' ')), out num)) //multi
                name = args.Substring(index + 1);
            else
                num = 1;

            ushort objType;
            if (!player.Manager.GameData.IdToObjectType.TryGetValue(name, out objType) ||
                !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
            {
                player.SendError("Unknown entity!");
                return false;
            }

            for (int i = 0; i < num; i++)
            {
                var entity = Entity.Resolve(player.Manager, objType);
                entity.Move(player.X, player.Y);
                player.Owner.EnterWorld(entity);
            }
            return true;
        }
    }
}
