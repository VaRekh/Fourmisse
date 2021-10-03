using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;


namespace Assets.Library.StateMachines.Seeker
{
    public class DumpState : State<StateCode, ControllerInfo>
    {

        public DumpState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }


        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Dump);
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
