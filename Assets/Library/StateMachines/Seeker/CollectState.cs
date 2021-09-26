using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class CollectState : State
    {

        public CollectState(StateUpdater updater)
            : base(updater)
        { }


        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Collect);
        }

        public override void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Idle:
                    state_updater.Change(StateCode.Return);
                    break;
                case CollectorStateCode.Collect:
                case CollectorStateCode.Dump:
                default:
                    break;
            }
        }
    }
}