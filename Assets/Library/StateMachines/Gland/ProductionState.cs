using UnityEngine;
using UnityEngine.Assertions;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.StateMachines.Gland
{
    public class ProductionState : State<StateCode, GlandInfo>
    {
        private Stopwatch stopwatch;

        public ProductionState(Updater<StateCode, GlandInfo> updater, GlandInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Production);
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);
        }

        public override void Update(float delta_time)
        {
            stopwatch.Update(delta_time);
            Assert.IsNotNull(Info.Pheromone);
            Assert.IsNotNull(Info.GenerationTransform);
            bool is_time_to_generate = stopwatch.CurrentValue >= Info.GenerationInterval;

            if (is_time_to_generate)
            {
                Object.Instantiate(Info.Pheromone, Info.GenerationTransform.position, Info.Pheromone.transform.rotation);
                stopwatch.Reset();
            }
        }

        public override void Exit()
        {
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        public void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Return:
                    break;
                default:
                    Updater.Change(StateCode.Idle);
                    break;
            }
        }
    }
}
