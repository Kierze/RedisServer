using log4net;
using wServer.realm.entities;
using wServer.realm.setpieces;

namespace wServer.realm.worlds
{
    internal class GameWorld : World
    {
        private static ILog log = LogManager.GetLogger(typeof(GameWorld));

        private bool oryxPresent;
        private int mapId;

        public GameWorld(int mapId, string name, bool oryxPresent)
        {
            Name = name;
            Background = 0;
            this.oryxPresent = oryxPresent;
            this.mapId = mapId;
        }

        public Oryx Overseer { get; private set; }

        protected override void Init()
        {
            log.Info($"Initializing Game World {Id} ({Name}) from map {mapId}");
            FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.world" + mapId + ".wmap"));
            SetPieces.ApplySetPieces(this);
            if (oryxPresent)
            {
                Overseer = new Oryx(this);
                Overseer.Init();
            }
            else
                Overseer = null;
            log.Info("Game World initalized.");
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            if (Overseer != null)
                Overseer.Tick(time);
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (Overseer != null)
                Overseer.OnEnemyKilled(enemy, killer);
        }

        public override int EnterWorld(Entity entity)
        {
            var ret = base.EnterWorld(entity);
            if (entity is Player)
                Overseer.OnPlayerEntered(entity as Player);
            return ret;
        }
    }
}
