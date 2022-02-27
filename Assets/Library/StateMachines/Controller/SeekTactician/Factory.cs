#nullable enable
using UnityEngine.Assertions;
using TacticianStateCode = Assets.Library.StateMachines.Controller.SeekTactician.StateCode;
using Assets.Library.StateMachines.Controller.SeekTactician.States;

namespace Assets.Library.StateMachines.Controller.SeekTactician
{
    public class Factory : IFactory<TacticianStateCode, SeekTacticianInfo>
    {
        public State<TacticianStateCode, SeekTacticianInfo> Get
        (
        TacticianStateCode code,
        Updater<TacticianStateCode, SeekTacticianInfo> updater,
        SeekTacticianInfo info
        )
        {
            State<TacticianStateCode, SeekTacticianInfo>? state = null;


            switch (code)
            {
                 case TacticianStateCode.Idle:
                    state = new IdleState(updater, info);
                    break;
                case TacticianStateCode.RandomMove:
                    state = new RandomMoveState(updater, info);
                    break;
                case TacticianStateCode.TrackPheromone:
                    state = new TrackPheromoneState(updater, info);
                    break;
                case TacticianStateCode.MoveBeyondLastPheromone:
                    state = new MoveBeyondLastPheromone(updater, info);
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
