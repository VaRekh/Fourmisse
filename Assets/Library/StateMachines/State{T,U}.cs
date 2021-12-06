using System;
using UnityEngine;
using Assets.Library.Data;

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

        public virtual void FixedUpdate(float delta_time)
        {

        }

        public virtual void Update(float delta_time)
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collision, params object[] data)
        {

        }

        public virtual void OnTriggerExit2D(Collider2D collision, params object[] data)
        {

        }

        protected static bool CheeckLayer(GameObject game_object, LayerReference layer)
        {
            int collided_layer = game_object.layer;
            int component_layer = layer.Index;
            bool is_same_layer = collided_layer == component_layer;

            return is_same_layer;
        }
    }
}
