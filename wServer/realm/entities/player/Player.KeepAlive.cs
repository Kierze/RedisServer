using System.Collections.Generic;
using wServer.networking.svrPackets;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private long lastPong = -1;
        private int? lastTime = null;
        private long tickMapping = 0;
        private Queue<long> ts = new Queue<long>();

        private const int PING_PERIOD = 5000;
        private const int DC_THRESOLD = 10000;

        private bool sentPing = false;

        private bool KeepAlive(RealmTime time)
        {
            if (lastPong == -1) lastPong = time.tickTimes - PING_PERIOD;
            if (time.tickTimes - lastPong > PING_PERIOD && !sentPing)
            {
                sentPing = true;
                ts.Enqueue(time.tickTimes);
                Client.SendPacket(new PingPacket());
            }
            else if (time.tickTimes - lastPong > DC_THRESOLD)
            {
                //client.Disconnect();
                return false;
            }
            return true;
        }

        internal void Pong(int time)
        {
            if (lastTime != null && (time - lastTime.Value > DC_THRESOLD || time - lastTime.Value < 0))
                ;//client.Disconnect();
            else
                lastTime = time;
            tickMapping = ts.Dequeue() - time;
            lastPong = time + tickMapping;
            sentPing = false;
            if (!Manager.Database.RenewLock(Client.Account))
                Client.Disconnect();
        }
    }
}
