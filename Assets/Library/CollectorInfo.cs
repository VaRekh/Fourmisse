using System;
using UnityEngine;

namespace Assets.Library
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

        public float Speed 
            => speed;

        public uint QuantityPerCollect
            => quantity_per_collect;

        public BoundedUint Load
        {
            get => load;
            set => load = value;
        }
    }
}
