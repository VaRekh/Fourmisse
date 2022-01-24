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
            UnityEvent<Collectable> contact_with_collectable_lost,
            UnityEvent<Storage> contact_with_storage_happened
        )
        {
            Assert.IsNotNull(serialized_info);
            _ = new StrictlyPositiveFloat(serialized_info.GeneratedPheromonePerSecond);
            Assert.IsNotNull(serialized_info.GenerationPosition);
            Assert.IsNotNull(serialized_info.PheromoneTemplate);
            Assert.IsNotNull(contact_with_collectable_lost);
            Assert.IsNotNull(contact_with_storage_happened);

            SerializedInfo = serialized_info;
            ContactWithCollectableLost = contact_with_collectable_lost;
            ContactWithStorageHappened = contact_with_storage_happened;

            Assert.IsNotNull(SerializedInfo);
            Assert.IsNotNull(ContactWithCollectableLost);
            Assert.IsNotNull(ContactWithStorageHappened);
        }
        
        private SerializedInfo SerializedInfo { get; set; }
        private UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
        private UnityEvent<Storage> ContactWithStorageHappened { get; set; }
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


        private Vector2 GenerationPosition
            => GenerationTransform.position;


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

        public GameObject InstantiatePheromone()
        {
            return GameObject.Instantiate
            (
                PheromoneTemplate,
                GenerationPosition,
                PheromoneTemplate.transform.rotation
            );
        }
    }
}