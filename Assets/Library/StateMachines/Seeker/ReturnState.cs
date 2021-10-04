using UnityEngine;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class ReturnState : State<StateCode, ControllerInfo>
    {
        public ReturnState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Return);
            Info.CollectorStateChanged.AddListener(ReactToCollectorStateChanged);
        }

        public override void Exit()
        {
            Info.CollectorStateChanged.RemoveListener(ReactToCollectorStateChanged);
        }
        /*
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            int collided_layer = collision.gameObject.layer;
            int anthill_layer = Info.AnthillLayer.Index;
            bool is_anthill_collided = collided_layer == anthill_layer;

            if (is_anthill_collided)
            {
                Updater.Change(StateCode.Seek);
            }
        }
        */
        public void ReactToCollectorStateChanged(CollectorStateCode new_state)
        {
            switch (new_state)
            {
                case CollectorStateCode.Dump:
                    Updater.Change(StateCode.Dump);
                    break;
                case CollectorStateCode.Idle:
                case CollectorStateCode.Collect:
                default:
                    break;
            }
        }
    }
}