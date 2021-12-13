#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.Data
{
    [Serializable]
    public class GlandInfo
    {
        public GlandInfo
        (
            StrictlyPositiveFloat generated_pheromone_per_second,
            Transform generation_position,
            GameObject pheromone_template,
            UnityEvent<Collectable> contact_with_collectable_lost,
            UnityEvent<Storage> contact_with_storage_happened
        )
        {
            Assert.IsNotNull(generation_position);
            Assert.IsNotNull(pheromone_template);
            Assert.IsNotNull(contact_with_collectable_lost);
            Assert.IsNotNull(contact_with_storage_happened);

            GeneratedPheromonePerSecond = generated_pheromone_per_second;
            GenerationTransform = generation_position;
            ContactWithCollectableLost = contact_with_collectable_lost;
            ContactWithStorageHappened = contact_with_storage_happened;
        }

        private Transform GenerationTransform { get; set; }
        private UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
        private UnityEvent<Storage> ContactWithStorageHappened { get; set; }

        public StrictlyPositiveFloat GeneratedPheromonePerSecond { get; private set; }
        
        public Vector2 GenerationPosition
            => GenerationTransform.position;

        public float PheromoneProductionTimeInSecond
            => 1f / GeneratedPheromonePerSecond;

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







        [SerializeField]
        private GameObject? pheromone;
        [SerializeField]
        private float pheromone_per_second;

        private UnityEvent<SeekerStateCode>? seeker_state_changed;
        private Transform? generation_transform;


        public GameObject? Pheromone
            => pheromone;

        public float GenerationInterval
            => 1f / pheromone_per_second;

        public UnityEvent<SeekerStateCode>? SeekerStateChanged
        {
            get => seeker_state_changed;
            set => seeker_state_changed = value ?? throw new ArgumentNullException();
        }
    }
}