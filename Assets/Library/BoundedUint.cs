using UnityEngine;
using System;

namespace Assets.Library
{
    [Serializable]
    public class BoundedUint
    {
        [SerializeField]
        private uint capacity;
        [SerializeField]
        private uint current_value;

        public uint Capacity
            => capacity;

        public uint CurrentValue
        {
            get => current_value;
            set => current_value = value < capacity ? value : capacity;
        }
        
        public BoundedUint(uint maximum_value = 0U)
        {
            capacity = maximum_value;
            current_value = 0U;
        }

        public void Increment(uint value)
        {
            CurrentValue += value;
        }
    }
}
