using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class SeekState : State
    {
        public SeekState(StateUpdater updater)
            : base(updater)
        { }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Seek);
        }

        public override void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Collect:
                    state_updater.Change(StateCode.Collect);
                    break;
                case CollectorStateCode.Idle:
                case CollectorStateCode.Dump:
                default:
                    break;
            }
        }
    }
}