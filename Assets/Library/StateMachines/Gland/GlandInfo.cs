#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.StateMachines.Gland
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
            PheromoneTemplate = pheromone_template;
            ContactWithCollectableLost = contact_with_collectable_lost;
            ContactWithStorageHappened = contact_with_storage_happened;

            Assert.IsNotNull(GenerationTransform);
            Assert.IsNotNull(PheromoneTemplate);
            Assert.IsNotNull(ContactWithCollectableLost);
            Assert.IsNotNull(ContactWithStorageHappened);
        }

        private Transform GenerationTransform { get; set; }
        private UnityEvent<Collectable> ContactWithCollectableLost { get; set; }
        private UnityEvent<Storage> ContactWithStorageHappened { get; set; }

        private StrictlyPositiveFloat GeneratedPheromonePerSecond { get; set; }
        
        private Vector2 GenerationPosition
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

        private GameObject PheromoneTemplate { get; set; }

        public GameObject InstantiatePheromone()
        {
            return GameObject.Instantiate
            (
                PheromoneTemplate,
                GenerationPosition,
                PheromoneTemplate.transform.rotation
            );
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