using Assets.Scripts;
using UnityEngine;

namespace Assets.Library.StateMachines.Collector
{
    public class IdleState : State<StateCode, CollectorInfo>
    {
        public IdleState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            var collectable = GetCollided<Collectable>(collision, Info.ResourceLayer);

            if (collectable != null)
            {
                Updater.Change(StateCode.Collect, collectable);
            }
            else
            {
                var anthill = GetCollided<Anthill>(collision, Info.AnthillLayer);

                if (anthill != null)
                {
                    Updater.Change(StateCode.Dump, anthill);
                }
            }
        }
    }
}