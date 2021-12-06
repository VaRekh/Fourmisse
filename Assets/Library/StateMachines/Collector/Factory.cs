using System;
using UnityEngine.Assertions;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Collector
{
    public class Factory : IFactory<StateCode, CollectorInfo>
    {
        public State<StateCode, CollectorInfo> Get(StateCode code, Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
        {
            State<StateCode, CollectorInfo> state = null;


            switch (code)
            {
                case StateCode.Idle:
                    state = new IdleState(updater, info);
                    break;
                case StateCode.Collect:
                    state = new CollectState(updater, info);
                    break;
                case StateCode.Dump:
                    state = new DumpState(updater, info);
                    break;
                default:
                    break;
            }

            Assert.IsNotNull(state);

            return state;
        }
    }
}
