#nullable enable
using UnityEngine.Assertions;
using Assets.Library.StateMachines.Gland.States;

namespace Assets.Library.StateMachines.Gland
{
    public class Factory : IFactory<StateCode, Info>
    {
        public State<StateCode, Info> Get(StateCode code, Updater<StateCode, Info> updater, Info info)
        {
            State<StateCode, Info>? state = null;


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

#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return state;
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }
    }
}
