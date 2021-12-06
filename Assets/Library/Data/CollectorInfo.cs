using System;
using UnityEngine;

namespace Assets.Library.Data
{
    [Serializable]
    public class CollectorInfo
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private uint quantity_per_collect;
        [SerializeField]
        BoundedUint load;
        [SerializeField]
        private LayerReference resource_layer;
        [SerializeField]
        private LayerReference anthill_layer;

        public float Speed 
            => speed;

        public uint QuantityPerCollect
            => quantity_per_collect;

        public BoundedUint Load
        {
            get => load;
            set => load = value;
        }

        public LayerReference ResourceLayer
            => resource_layer;
        public LayerReference AnthillLayer
            => anthill_layer;
    }
}
