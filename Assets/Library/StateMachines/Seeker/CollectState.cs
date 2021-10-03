using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class CollectState : State<StateCode, ControllerInfo>
    {

        public CollectState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }


        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Collect);
        }

        public void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Idle:
                    Updater.Change(StateCode.Return);
                    break;
                case CollectorStateCode.Collect:
                case CollectorStateCode.Dump:
                default:
                    break;
            }
        }
    }
}