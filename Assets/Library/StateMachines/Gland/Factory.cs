using UnityEngine.Assertions;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Gland
{
    public class Factory : IFactory<StateCode, GlandInfo>
    {
        public State<StateCode, GlandInfo> Get(StateCode code, Updater<StateCode, GlandInfo> updater, GlandInfo info)
        {
            State<StateCode, GlandInfo> state = null;


            switch (code)
            {
                case StateCode.Idle:
                    state = new IdleState(updater, info);
                    break;
                case StateCode.Production:
                    state = new ProductionState(updater, info);
                    break;
                default:
                    break;
            }

            Assert.IsNotNull(state);

            return state;
        }
    }
}
