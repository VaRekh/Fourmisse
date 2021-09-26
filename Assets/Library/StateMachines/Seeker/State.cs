using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class State : State<StateUpdater>
    {
            public State(StateUpdater updater)
        : base(updater)
            { }

        public virtual void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {

        }
    }
}
