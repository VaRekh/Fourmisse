#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library.StateMachines.Gland
{
    [Serializable]
    public class SerializedInfo
    {
        [SerializeField]
        public GameObject? PheromoneTemplate;

        [SerializeField]
        [Range(float.Epsilon, float.MaxValue)]
        public float GeneratedPheromonePerSecond;

        [SerializeField]
        public Transform? GenerationPosition;

        public Info Build
        (
            UnityEvent<Collectable> contact_with_collectable_lost,
            UnityEvent<Storage> contact_with_storage_happened
        )
        {
            var info = new Info
            (
                this,
                contact_with_collectable_lost,
                contact_with_storage_happened
            );

            return info;
        }
    }
}