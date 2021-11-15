using UnityEngine.Assertions;
using TacticianStateCode = Assets.Library.StateMachines.Controller.SeekTactician.StateCode;

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
            State<TacticianStateCode, SeekTacticianInfo> state = null;


            switch (code)
            {
                 case TacticianStateCode.Idle:
                    state = new SeekTactician.IdleState(updater, info);
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

            return state;
        }
    }
}
