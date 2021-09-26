using Assets.Scripts;
using UnityEngine;

namespace Assets.Library.StateMachines.Collector
{
    public class IdleState : State
    {
        public IdleState(StateUpdater updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Idle);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            int collided_layer = collision.gameObject.layer;

            var collectable = GetResourceCollided(collided_layer);

            if (collectable != null)
            {
                state_updater.Change(StateCode.Collect, collectable);
            }
            else
            {
                var anthill = GetAnthillCollided(collided_layer);

                if (anthill != null)
                {
                    state_updater.Change(StateCode.Dump, anthill);
                }
            }


            Collectable GetResourceCollided(int collided_layer)
            {
                int resource_layer = LayerMask.NameToLayer("Resources");
                bool is_resource_collided = collided_layer == resource_layer;
                Collectable collectable = null;

                if (is_resource_collided)
                {
                    collectable = collision.gameObject.GetComponent<Collectable>();
                }

                return collectable;
            }

            Anthill GetAnthillCollided(int collided_layer)
            {
                int anthill_layer = LayerMask.NameToLayer("Anthill");
                bool is_anthill_collided = collided_layer == anthill_layer;
                Anthill anthill = null;

                if (is_anthill_collided)
                {
                    anthill = collision.gameObject.GetComponent<Anthill>();
                }

                return anthill;
            }

            
        }
    }
}