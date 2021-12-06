﻿using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Seeker
{
    public class CollectState : State<StateCode, SeekerInfo>
    {

        public CollectState(Updater<StateCode, SeekerInfo> updater, SeekerInfo info)
            : base(updater, info)
        { }


        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Collect);
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