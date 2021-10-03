﻿using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class SeekState : State<StateCode, ControllerInfo>
    {
        public SeekState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        {
            info.CollectorStateChanged.AddListener(ReactToCollectorStateChanged);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Seek);
        }

        public void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Collect:
                    Updater.Change(StateCode.Collect);
                    break;
                case CollectorStateCode.Idle:
                case CollectorStateCode.Dump:
                default:
                    break;
            }
        }
    }
}