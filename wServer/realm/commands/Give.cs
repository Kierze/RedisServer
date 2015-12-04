using wServer.realm.entities;

namespace wServer.realm.commands
{
    internal class CommandGive : Command
    {
        public CommandGive() : base("give", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ushort objType;
            if (!player.Manager.GameData.IdToObjectType.TryGetValue(args, out objType))
            {
                player.SendError("Unknown item type!");
                return false;
            }
            for (int i = 0; i < player.Inventory.Length; i++)
                if (player.Inventory[i] == null)
                {
                    player.Inventory[i] = player.Manager.GameData.Items[objType];
                    player.UpdateCount++;
                    return true;
                }
            player.SendError("Not enough space in inventory!");
            return false;
        }
    }
}
