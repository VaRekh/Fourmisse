using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class State : State<StateUpdater>
    {
        protected ControllerInfo Info { get; private set; }

        public State(StateUpdater updater, ControllerInfo info)
        : base(updater)
        {
            Info = info;
        }

        public virtual void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {

        }
    }
}
