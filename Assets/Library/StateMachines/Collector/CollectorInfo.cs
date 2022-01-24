using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library.StateMachines.Collector
{
    [Serializable]
    public class CollectorInfo
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private uint quantity_per_collect;
        [SerializeField]
        private BoundedUint load;
        [SerializeField]
        private LayerReference resource_layer;
        [SerializeField]
        private LayerReference anthill_layer;

        public float Speed
            => speed;

        public uint QuantityPerCollect
            => quantity_per_collect;

        public BoundedUint Load
        {
            get => load;
            set => load = value;
        }

        public LayerReference ResourceLayer
            => resource_layer;
        public LayerReference AnthillLayer
            => anthill_layer;

        private UnityEvent<Collectable> ContactWithNonEmptyCollectableHappenned { get; set; }

        private UnityEvent<Storage> ContactWithStorageHappened { get; set; }

        public void ListenToCollectorCompletelyEmptied(UnityAction listener)
        {
            Load.ListenToBecameEmpty(listener);
        }

        public void StopListeningToCollectorCompletelyEmptied(UnityAction listener)
        {
            Load.StopListeningToBecameEmpty(listener);
        }

        public void ListenToCollectorCompletelyLoaded(UnityAction listener)
        {
            Load.ListenToBecameFull(listener);
        }

        public void StopListeningToCollectorCompletelyLoaded(UnityAction listener)
        {
            Load.StopListeningToBecameFull(listener);
        }

        public void ListenToContactWithNonEmptyCollectableHappenned(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappenned.AddListener(listener);
        }

        public void StopListeningToContactWithNonEmptyCollectableHappenned(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappenned.RemoveListener(listener);
        }

        public void ListenToContactWithStorageHappened(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.AddListener(listener);
        }

        public void StopListeningToContactWithStorageHappened(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.RemoveListener(listener);
        }

        public void Build
        (
            UnityEvent<Collectable> contact_with_non_empty_collectable_happened,
            UnityEvent<Storage> contact_with_storage_happened
        )
        {
            ContactWithNonEmptyCollectableHappenned = contact_with_non_empty_collectable_happened;
            ContactWithStorageHappened = contact_with_storage_happened;
        }
    }
}
