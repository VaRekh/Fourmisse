#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library.StateMachines.Gland
{
    [Serializable]
    public struct SerializedInfo
    {
        [SerializeField]
        public GameObject? PheromoneTemplate;

        [SerializeField]
        [Range(float.Epsilon, float.MaxValue)]
        public float GeneratedPheromonePerSecond;

        [SerializeField]
        public Transform? GenerationPosition;

        [SerializeField]
        public UnityEvent<Collectable>? ContactWithCollectableLost;

        [SerializeField]
        public UnityEvent<Storage>? ContactWithStorageHappened;

        public Info Build()
        {
            var generated_pheromone_per_second = new StrictlyPositiveFloat(GeneratedPheromonePerSecond);
            var info = new Info
            (
                generated_pheromone_per_second,
                GenerationPosition,
                PheromoneTemplate,
                ContactWithCollectableLost,
                ContactWithStorageHappened
            );

            return info;
        }
    }
}