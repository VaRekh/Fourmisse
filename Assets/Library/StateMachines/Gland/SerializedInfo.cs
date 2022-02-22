#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

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

        public Info Build(NonSerializedInfo non_serialized_info)
        {
            var info = new Info
            (
                this,
                non_serialized_info
            );

            return info;
        }
        public void CheckValidity()
        {
            _ = new StrictlyPositiveFloat(GeneratedPheromonePerSecond);
            Assert.IsNotNull(GenerationPosition);
            Assert.IsNotNull(PheromoneTemplate);
        }
    }
}