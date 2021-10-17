using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;


namespace Assets.Library.StateMachines.Seeker
{
    public class DumpState : State<StateCode, SeekerInfo>
    {

        public DumpState(Updater<StateCode, SeekerInfo> updater, SeekerInfo info)
            : base(updater, info)
        { }


        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Dump);
            Info.CollectorStateChanged.AddListener(ReactToCollectorStateChanged);
        }

        public override void Exit()
        {
            Info.CollectorStateChanged.RemoveListener(ReactToCollectorStateChanged);
        }

        public void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Idle:
                    Updater.Change(StateCode.Seek);
                    break;
                default:
                    break;
            }
        }
    }
}
