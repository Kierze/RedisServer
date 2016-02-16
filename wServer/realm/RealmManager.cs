using common;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using wServer.logic;
using wServer.networking;
using wServer.realm.commands;
using wServer.realm.worlds;

namespace wServer.realm
{
    public struct RealmTime
    {
        public long tickCount;
        public long tickTimes;
        public int thisTickCounts;
        public int thisTickTimes;
    }

    public class TimeEventArgs : EventArgs
    {
        public TimeEventArgs(RealmTime time)
        {
            Time = time;
        }

        public RealmTime Time { get; private set; }
    }

    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Normal,
        Creation,
    }

    public class RealmManager
    {
        private static ILog log = LogManager.GetLogger(typeof(RealmManager));

        public string InstanceId { get; private set; }
        public int MaxClient { get; private set; }
        public int TPS { get; private set; }
        public Database Database { get; private set; }

        public RealmManager(int maxClient, int tps, Database db)
        {
            InstanceId = Guid.NewGuid().ToString();
            MaxClient = maxClient;
            TPS = tps;
            Database = db;
        }

        private int nextWorldId = 0;
        private int nextClientId = 0;
        public readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        public readonly ConcurrentDictionary<int, Client> Clients = new ConcurrentDictionary<int, Client>();
        public ConcurrentDictionary<string, World> PlayerWorldMapping = new ConcurrentDictionary<string, World>();

        public RealmPortalMonitor Monitor { get; private set; }

        public bool TryConnect(Client client)
        {
            if (Clients.Count >= MaxClient)
                return false;
            else
            {
                client.Id = Interlocked.Increment(ref nextClientId);
                Clients[client.Id] = client;
                return true;
            }
        }

        public void Disconnect(Client client)
        {
            Clients.TryRemove(client.Id, out client);
        }

        public World AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = id;
            Worlds[id] = world;
            OnWorldAdded(world);
            return world;
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            OnWorldAdded(world);
            return world;
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");
            if (Worlds.TryRemove(world.Id, out world))
            {
                OnWorldRemoved(world);
                return true;
            }
            else
                return false;
        }

        public World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        private void OnWorldAdded(World world)
        {
            world.Manager = this;
            if (world is GameWorld)
                Monitor.WorldAdded(world);
            log.Info($"World {world.Id} ({world.Name}) added");
        }

        private void OnWorldRemoved(World world)
        {
            world.Manager = null;
            if (world is GameWorld)
                Monitor.WorldRemoved(world);
            log.Info($"World {world.Id} ({world.Name}) removed");
        }

        private Thread network;
        public NetworkTicker Network { get; private set; }

        private Thread logic;
        public LogicTicker Logic { get; private set; }

        public XmlData GameData { get; private set; }
        public BehaviorDb Behaviors { get; private set; }

        public ISManager InterServer { get; private set; }
        public ChatManager Chat { get; private set; }
        public CommandManager Commands { get; private set; }

        public void Initialize()
        {
            log.Info("Initializing Realm Manager...");

            GameData = new XmlData();
            Behaviors = new BehaviorDb(this);

            AddWorld(World.NEXUS_ID, Worlds[0] = new Nexus());
            Monitor = new RealmPortalMonitor(this);

            AddWorld(World.TUT_ID, new Tutorial(true));
            AddWorld(World.NEXUS_LIMBO, new NexusLimbo());
            AddWorld(World.VAULT_ID, new Vault(true));
            AddWorld(World.TEST_ID, new Test());
            AddWorld(World.RAND_REALM, new RandomRealm());
            AddWorld(World.GAUNTLET, new GauntletMap());

            //AddWorld(new GameWorld(1, "Medusa", true));

            InterServer = new ISManager(this);
            Chat = new ChatManager(this);
            Commands = new CommandManager(this);

            log.Info("Realm Manager initialized.");
        }

        public void Run()
        {
            log.Info("Starting Realm Manager...");

            Network = new NetworkTicker(this);
            Logic = new LogicTicker(this);
            network = new Thread(Network.TickLoop)
            {
                Name = "Network",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            logic = new Thread(Logic.TickLoop)
            {
                Name = "Logic",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            //Start logic loop first
            logic.Start();
            network.Start();

            log.Info("Realm Manager started.");
        }

        public bool Terminating { get; private set; }

        public void Stop()
        {
            log.Info("Stopping Realm Manager...");

            InterServer.Dispose();
            Terminating = true;
            GameData.Dispose();
            logic.Join();
            network.Join();

            log.Info("Realm Manager stopped.");
        }
    }
}
