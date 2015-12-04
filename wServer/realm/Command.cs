using log4net;
using System;
using System.Collections.Generic;
using wServer.realm.entities;

namespace wServer.realm
{
    public abstract class Command
    {
        private static ILog log = LogManager.GetLogger(typeof(Command));

        public Command(string name, int permLevel = 0)
        {
            this.CommandName = name;
            this.PermissionLevel = permLevel;
        }

        public string CommandName { get; private set; }

        public int PermissionLevel { get; private set; }

        protected abstract bool Process(Player player, RealmTime time, string args);

        private static int GetPermissionLevel(Player player)
        {
            if (player.Client.Account.Admin)
                return 1;
            else
                return 0;
        }

        public bool HasPermission(Player player)
        {
            if (GetPermissionLevel(player) < PermissionLevel)
                return false;
            return true;
        }

        public bool Execute(Player player, RealmTime time, string args)
        {
            if (!HasPermission(player))
            {
                player.SendError("No permission!");
                return false;
            }

            try
            {
                return Process(player, time, args);
            }
            catch (Exception ex)
            {
                log.Error("Error when executing the command.", ex);
                player.SendError("Error when executing the command.");
                return false;
            }
        }
    }

    public class CommandManager
    {
        private static ILog log = LogManager.GetLogger(typeof(CommandManager));

        private Dictionary<string, Command> cmds;
        public IDictionary<string, Command> Commands { get { return cmds; } }

        private RealmManager manager;

        public CommandManager(RealmManager manager)
        {
            this.manager = manager;
            cmds = new Dictionary<string, Command>(StringComparer.InvariantCultureIgnoreCase);
            var t = typeof(Command);
            foreach (var i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    var instance = (Command)Activator.CreateInstance(i);
                    cmds.Add(instance.CommandName, instance);
                }
        }

        public bool Execute(Player player, RealmTime time, string text)
        {
            var index = text.IndexOf(' ');
            string cmd = text.Substring(1, index == -1 ? text.Length - 1 : index - 1);
            var args = index == -1 ? "" : text.Substring(index + 1);

            Command command;
            if (!cmds.TryGetValue(cmd, out command))
            {
                player.SendError("Unknown command!");
                return false;
            }
            else
            {
                log.InfoFormat("[Command] <{0}> {1}", player.Name, text);
                return command.Execute(player, time, args);
            }
        }
    }
}
