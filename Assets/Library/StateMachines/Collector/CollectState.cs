using UnityEngine;
using Assets.Scripts;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector
{
    public class CollectState : State
    {
        private Stopwatch stopwatch;
        private Collectable collectable;

        private float CollectingInterval
            => 1f / Info.Speed;

        public CollectState(StateUpdater updater, CollectorInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(0f);
        }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Collect);
            stopwatch.Reset();

            bool is_data_available = data.Length > 0;
            collectable = GetCollectable(data[0]);


            Collectable GetCollectable(object data)
            {

                Collectable collectable_found = is_data_available ? data as Collectable : null;

                Assert.IsNotNull(collectable_found);

                return collectable_found;
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

                uint load_acquired = collectable.Collect(Info.QuantityPerCollect);
                Info.Load.Increment(load_acquired);
                is_collectable_empty = load_acquired == 0U;
            }

            bool is_capacity_full = Info.Load.CurrentValue == Info.Load.Capacity;

            if (is_capacity_full || is_collectable_empty)
            {
                state_updater.Change(StateCode.Idle);
            }
        }


        public override void OnTriggerExit2D(Collider2D collision)
        {
            state_updater.Change(StateCode.Idle);
        }
    }
}