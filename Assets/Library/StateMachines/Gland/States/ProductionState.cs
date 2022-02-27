#nullable enable
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Gland.States
{
    public class ProductionState : State<StateCode, Info>
    {
        private readonly Stopwatch stopwatch;
        private Collectable? collectable;

        public ProductionState(Updater<StateCode, Info> updater, Info info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Production);

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);

            collectable = GetCollectable(data[0]);

            static Collectable? GetCollectable(object data)
            {
                Collectable? collectable_found = data as Collectable;
                Assert.IsNotNull(collectable_found);

                return collectable_found;
            }

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
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
                var intensity = new Intensity(collectable.LoadLeft);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
                Info.InstantiatePheromone(intensity);
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
