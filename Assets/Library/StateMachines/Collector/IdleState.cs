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
            int collided_layer = collision.gameObject.layer;

            var collectable = GetCollided<Collectable>(collision, collided_layer, Info.ResourceLayer);

            if (collectable != null)
            {
                Updater.Change(StateCode.Collect, collectable);
            }
            else
            {
                var anthill = GetCollided<Anthill>(collision, collided_layer, Info.AnthillLayer);

                if (anthill != null)
                {
                    Updater.Change(StateCode.Dump, anthill);
                }
            }

            static TComponent GetCollided<TComponent>(Collider2D collision, int collided_layer, LayerReference layer)
                where TComponent : Component
            {
                int component_layer = layer.Index;
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