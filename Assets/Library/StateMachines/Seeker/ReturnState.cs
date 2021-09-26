using UnityEngine;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class ReturnState : State
    {
        public ReturnState(StateUpdater updater)
            : base(updater)
        { }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Return);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            int collided_layer = collision.gameObject.layer;
            int base_layer = LayerMask.NameToLayer("Base");
            bool is_base_collided = collided_layer == base_layer;

            if (is_base_collided)
            {
                state_updater.Change(StateCode.Seek);
            }
        }

        public override void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Dump:
                    state_updater.Change(StateCode.Dump);
                    break;
                case CollectorStateCode.Idle:
                case CollectorStateCode.Collect:
                default:
                    break;
            }
        }
    }
}