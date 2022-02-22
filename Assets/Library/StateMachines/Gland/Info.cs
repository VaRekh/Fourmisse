#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Gland
{
    [Serializable]
    public class Info
    {
        public Info
        (
            SerializedInfo serialized_info,
            NonSerializedInfo non_serialized_info
        )
        {
            Assert.IsNotNull(serialized_info);
            serialized_info.CheckValidity();
            non_serialized_info.CheckValidity();

            SerializedInfo = serialized_info;
            NonSerializedInfo = non_serialized_info;

            Assert.IsNotNull(SerializedInfo);
            SerializedInfo.CheckValidity();
            NonSerializedInfo.CheckValidity();
        }
        
        private SerializedInfo SerializedInfo { get; set; }
        private NonSerializedInfo NonSerializedInfo { get; set; }
        private UnityEvent<Collectable> ContactWithCollectableLost
            => NonSerializedInfo.ContactWithCollectableLost;
        private UnityEvent<Storage> ContactWithStorageHappened
            => NonSerializedInfo.ContactWithStorageHappened;
        private UnityEvent<Collectable> ContactWithNonEmptyCollectableHappened
            => NonSerializedInfo.ContactWithNonEmptyCollectableHappened;
        private BoundedUint Load
            => NonSerializedInfo.Load;
        private UnityAction<GameObject, Identifier, float> InitPheromone
            => NonSerializedInfo.InitPheromone;
        private Identifier Identifier
            => NonSerializedInfo.Identifier;

        private GameObject PheromoneTemplate
        {
            get
            {
                Assert.IsNotNull(SerializedInfo.PheromoneTemplate);
                return SerializedInfo.PheromoneTemplate;
            }
        }
        private Transform GenerationTransform
        {
            get
            {
                Assert.IsNotNull(SerializedInfo.GenerationPosition);
                return SerializedInfo.GenerationPosition;
            }
        }
        private float GeneratedPheromonePerSecond
        {
            get
            {
                var generated_pheromone_per_second = new StrictlyPositiveFloat(SerializedInfo.GeneratedPheromonePerSecond);
                return generated_pheromone_per_second;
            }
        }

        public float PheromoneProductionTimeInSecond
            => 1f / GeneratedPheromonePerSecond;

        public bool CollectorIsNotFull
            => Load.IsNotFull;


        private Vector2 GenerationPosition
            => GenerationTransform.position;


        public void ListenToContactWithNonEmptyCollectableHappened(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappened.AddListener(listener);
        }

        public void StopListeningToContactWithNonEmptyCollectableHappened(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappened.RemoveListener(listener);
        }

        public void ListenToLossOfContactWithCollectable(UnityAction<Collectable> listener)
        {
            ContactWithCollectableLost.AddListener(listener);
        }

        public void StopListeningToLossOfContactWithCollectable(UnityAction<Collectable> listener)
        {
            ContactWithCollectableLost.RemoveListener(listener);
        }

        public void ListenToContactWithStorage(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.AddListener(listener);
        }

        public void StopListeningToContactWithStorage(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.RemoveListener(listener);
        }

        public GameObject InstantiatePheromone(float intensity)
        {
            var pheromone =  GameObject.Instantiate
            (
                PheromoneTemplate,
                GenerationPosition,
                PheromoneTemplate.transform.rotation
            );

            InitPheromone(pheromone, Identifier, intensity);
            return pheromone;
        }
    }
}