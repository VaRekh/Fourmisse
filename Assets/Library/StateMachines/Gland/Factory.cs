using UnityEngine.Assertions;
using Assets.Library.Data;
using Assets.Library.StateMachines.Gland.States;

namespace Assets.Library.StateMachines.Gland
{
    public class Factory : IFactory<StateCode, Info>
    {
        public State<StateCode, Info> Get(StateCode code, Updater<StateCode, Info> updater, Info info)
        {
            State<StateCode, Info> state = null;


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
