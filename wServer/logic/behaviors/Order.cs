using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class Order : Behavior
    {
        //State storage: none

        private double range;
        private ushort children;
        private string targetStateName;
        private State targetState;

        public Order(double range, string children, string targetState)
        {
            this.range = range;
            this.children = BehaviorDb.InitGameData.IdToObjectType[children];
            this.targetStateName = targetState;
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name) return state;
            State ret;
            foreach (var i in state.States)
            {
                if ((ret = FindState(i, name)) != null)
                    return ret;
            }
            return null;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (targetState == null)
                this.targetState = FindState(host.Manager.Behaviors.Definitions[this.children].Item1, targetStateName);
            foreach (var i in host.GetNearestEntities(range, children))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}
