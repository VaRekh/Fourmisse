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

            var collectable = GetCollided<Collectable>(collision, collided_layer, "Resources");

            if (collectable != null)
            {
                state_updater.Change(StateCode.Collect, collectable);
            }
            else
            {
                var anthill = GetCollided<Anthill>(collision, collided_layer, "Anthill");

                if (anthill != null)
                {
                    state_updater.Change(StateCode.Dump, anthill);
                }
            }

            static TComponent GetCollided<TComponent>(Collider2D collision, int collided_layer, string layer_name)
                where TComponent : Component
            {
                int component_layer = LayerMask.NameToLayer(layer_name);
                bool is_layer_collided = collided_layer == component_layer;
                TComponent component = null;

                if (is_layer_collided)
                {
                    component = collision.gameObject.GetComponent<TComponent>();
                }

                return component;
            }
        }
    }
}