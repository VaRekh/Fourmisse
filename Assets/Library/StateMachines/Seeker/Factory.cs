using System;
using UnityEngine.Assertions;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Seeker
{
    public class Factory : IFactory<StateCode, SeekerInfo>
    {
        public State<StateCode, SeekerInfo> Get(StateCode code, Updater<StateCode, SeekerInfo> updater, SeekerInfo info)
        {
            State<StateCode, SeekerInfo> state = null;


            switch (code)
            {
                case StateCode.Seek:
                    state = new SeekState(updater, info);
                    break;
                case StateCode.Collect:
                    state = new CollectState(updater, info);
                    break;
                case StateCode.Return:
                    state = new ReturnState(updater, info);
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
