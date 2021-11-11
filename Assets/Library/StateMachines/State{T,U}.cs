using System;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Library.StateMachines
{
    public abstract class State<TStateCode, TInfo>
        where TStateCode : Enum
    {
        protected Updater<TStateCode, TInfo> Updater { get; private set; }
        protected TInfo Info { get; private set; }

        public State(Updater<TStateCode, TInfo> updater, TInfo info)
        {
            Updater = updater;
            Info = info;
        }


        public virtual void Enter(params object[] data)
        {

        }

        public virtual void Update(float delta_time)
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {

        }

        protected static TComponent GetCollided<TComponent>(Collider2D collision, LayerReference layer)
            where TComponent : Component
        {

            int collided_layer = collision.gameObject.layer;
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
