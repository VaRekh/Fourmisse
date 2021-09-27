using UnityEngine;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class ReturnState : State
    {
        public ReturnState(StateUpdater updater, ControllerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Return);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            int collided_layer = collision.gameObject.layer;
            int anthill_layer = Info.AnthillLayer.Index;
            bool is_anthill_collided = collided_layer == anthill_layer;

            if (is_anthill_collided)
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