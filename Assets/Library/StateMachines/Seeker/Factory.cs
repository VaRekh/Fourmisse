using System;

namespace Assets.Library.StateMachines.Seeker
{
    public class Factory : IFactory<StateCode, ControllerInfo>
    {
        public State<StateCode, ControllerInfo> Get(StateCode code, Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
        {
            State<StateCode, ControllerInfo> state = null;


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

            return state;
        }
    }
}
