#nullable enable
namespace Assets.Library.StateMachines.Gland.States
{
    public class ProductionState : State<StateCode, Info>
    {
        private readonly Stopwatch stopwatch;

        public ProductionState(Updater<StateCode, Info> updater, Info info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Production);
            Info.ListenToContactWithStorage(OnContactWithStorage);
            Info.ListenToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);
            stopwatch.Reset(Info.PheromoneProductionTimeInSecond);
        }

        public override void Update(float delta_time)
        {
            stopwatch.Update(delta_time);
            bool is_time_to_generate = stopwatch.CurrentValue >= Info.PheromoneProductionTimeInSecond;

            if (is_time_to_generate)
            {
                Info.InstantiatePheromone(10f);
                stopwatch.Reset();
            }
        }

        public override void Exit()
        {
            Info.StopListeningToContactWithStorage(OnContactWithStorage);
            Info.StopListeningToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);
        }
        
        private void OnContactWithStorage(Storage storage)
        {
            Updater.Change(StateCode.Idle);
        }

        private void OnContactWithNonEmptyCollectableHappened(Collectable collectable)
        {
            if (Info.CollectorIsNotFull)
            {
                Updater.Change(StateCode.Idle);
            }
        }
    }
}
