#nullable enable
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector.States
{
    public class CollectState : State<StateCode, CollectorInfo>
    {
        private readonly Stopwatch stopwatch;
        private Collectable? collectable;

        private float CollectingInterval
            => 1f / Info.Speed;

        public CollectState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Collect);
            stopwatch.Reset();

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);
            collectable = GetCollectable(data[0]);

            static Collectable GetCollectable(object data)
            {
                Collectable? collectable_found = data as Collectable;
                Assert.IsNotNull(collectable_found);

#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
                return collectable_found;
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
            }
        }

        public override void Update(float delta_time)
        {
            stopwatch.Update(delta_time);

            bool is_time_to_collect = stopwatch.CurrentValue >= CollectingInterval;
            bool is_collectable_empty = false;

            if (is_time_to_collect)
            {
                stopwatch.Reset();

#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
                uint load_acquired = collectable.Collect(Info.QuantityPerCollect);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
                Info.Load.Increment(load_acquired);
                is_collectable_empty = load_acquired == 0U;
            }

            bool is_capacity_full = Info.Load.IsFull;

            if (is_capacity_full || is_collectable_empty)
            {
                Updater.Change(StateCode.Idle);
            }
        }
    }
}