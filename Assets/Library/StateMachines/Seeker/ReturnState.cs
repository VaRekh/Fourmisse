using UnityEngine;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Seeker
{
    public class ReturnState : State<StateCode, SeekerInfo>
    {
        public ReturnState(Updater<StateCode, SeekerInfo> updater, SeekerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Return);
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
                case CollectorStateCode.Dump:
                    Updater.Change(StateCode.Dump);
                    break;
                case CollectorStateCode.Idle:
                case CollectorStateCode.Collect:
                default:
                    break;
            }
        }
    }
}