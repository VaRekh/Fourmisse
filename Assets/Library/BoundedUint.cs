using UnityEngine;
using UnityEngine.Events;
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

        [SerializeField]
        private UnityEvent became_empty = new();
        [SerializeField]
        private UnityEvent became_full = new();

        public bool IsFull
            => current_value == capacity;

        public bool IsNotFull
            => !IsFull;

        public bool IsEmpty
            => current_value == 0U;

        public bool IsNotEmpty
            => !IsEmpty;

        public uint CurrentValue
        {
            get => current_value;
            set
            {
                current_value = value < capacity ? value : capacity;
                if (current_value == 0U)
                {
                    became_empty.Invoke();
                }
                else if (current_value == capacity)
                {
                    became_full.Invoke();
                }
            }
        }

        public void ListenToBecameEmpty(UnityAction listener)
        {
            became_empty.AddListener(listener);
        }

        public void StopListeningToBecameEmpty(UnityAction listener)
        {
            became_empty.RemoveListener(listener);
        }

        public void ListenToBecameFull(UnityAction listener)
        {
            became_full.AddListener(listener);
        }

        public void StopListeningToBecameFull(UnityAction listener)
        {
            became_full.RemoveListener(listener);
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
