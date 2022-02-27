#nullable enable
using UnityEngine.Assertions;
using Assets.Library.StateMachines.Controller.States;

namespace Assets.Library.StateMachines.Controller
{
    public class Factory : IFactory<StateCode, ControllerInfo>
    {
        public State<StateCode, ControllerInfo> Get(StateCode code, Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
        {
            State<StateCode, ControllerInfo>? state = null;


            switch (code)
            {
                case StateCode.Idle:
                    state = new IdleState(updater, info);
                    break;
                case StateCode.Seek:
                    state = new SeekState(updater, info);
                    break;
                case StateCode.Return:
                    state = new ReturnState(updater, info);
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
