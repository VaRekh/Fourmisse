using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;


namespace Assets.Library.StateMachines.Seeker
{
    public class DumpState : State
    {

        public DumpState(StateUpdater updater)
            : base(updater)
        { }


        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Dump);
        }

        public override void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Idle:
                    state_updater.Change(StateCode.Seek);
                    break;
                default:
                    break;
            }
        }
    }
}
