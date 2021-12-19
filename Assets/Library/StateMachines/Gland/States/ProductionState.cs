#nullable enable
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Gland.States
{
    public class ProductionState : State<StateCode, GlandInfo>
    {
        private readonly Stopwatch stopwatch;

        public ProductionState(Updater<StateCode, GlandInfo> updater, GlandInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Production);
            Info.ListenToContactWithStorage(OnContact);
            stopwatch.Reset(Info.PheromoneProductionTimeInSecond);
        }

        public override void Update(float delta_time)
        {
            stopwatch.Update(delta_time);
            bool is_time_to_generate = stopwatch.CurrentValue >= Info.PheromoneProductionTimeInSecond;

            if (is_time_to_generate)
            {
                Info.InstantiatePheromone();
                stopwatch.Reset();
            }
        }

        public override void Exit()
        {
            Info.StopListeningToContactWithStorage(OnContact);
        }
        
        private void OnContact(Storage storage)
        {
            Updater.Change(StateCode.Idle);
        }
    }
}
