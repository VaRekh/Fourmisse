using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library.StateMachines.Controller.SeekTactician;

namespace Assets.Library.StateMachines.Controller
{
    [Serializable]
    public class ControllerInfo
    {
        [Serializable]
        public class SharedInfo
        {
            [SerializeField]
            private float movespeed;

            private Transform ant;
            private Rigidbody2D rigidbody;


            public float Movespeed
                => movespeed;

            private Transform Ant
            {
                set
                {
                    Assert.IsNotNull(value);
                    ant = value;
                }
            }

            public Vector2 AntPosition
            {
                get
                {
                    Assert.IsNotNull(ant);
                    return ant.position;
                }
            }

            public Rigidbody2D Rigidbody
            {
                get => rigidbody;
                private set
                {
                    Assert.IsNotNull(value);
                    rigidbody = value;
                }
            }

            public void Init(Rigidbody2D rigidbody, Transform ant)
            {
                Rigidbody = rigidbody;
                Ant = ant;
            }
        }

        [SerializeField]
        private SharedInfo shared_info;

        [SerializeField]
        private SeekTacticianInfo seek_tactician_info;
        [SerializeField]
        private Transform anthill;
        [SerializeField][HideInInspector]
        private Detector pheromone_detector;

        public float Movespeed
            => shared_info.Movespeed;

        public SeekTacticianInfo SeekTacticianInfo
            => seek_tactician_info;

        public Transform Anthill
        { 
            set
            {
                anthill = value;
                Assert.IsNotNull(anthill);
            }
        }

        public Vector2 AnthillPosition
        {
            get
            {
                Assert.IsNotNull(anthill);
                return anthill.position;
            }
        }

        public Vector2 AntPosition
            => shared_info.AntPosition;

        public Rigidbody2D Rigidbody
            => shared_info.Rigidbody;

        #region Subscribe/Unsubscribed
        private UnityEvent<Collectable> ContactWithNonEmptyCollectableHappened { get; set; }

        public void ListenToContactWithNonEmptyCollectableHappened(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappened.AddListener(listener);
        }

        public void StopListeningToContactWithNonEmptyCollectableHappened(UnityAction<Collectable> listener)
        {
            ContactWithNonEmptyCollectableHappened.RemoveListener(listener);
        }


        private UnityEventSubscription CollectorCompletelyEmptied { get; set; }

        public void ListenToCollectorCompletelyEmptied(UnityAction listener)
        {
            CollectorCompletelyEmptied.ListenToUnityEvent(listener);
        }

        public void StopListeningToCollectorCompletelyEmptied(UnityAction listener)
        {
            CollectorCompletelyEmptied.StopListeningToUnityEvent(listener);
        }


        private UnityEventSubscription CollectorCompletelyLoaded { get; set; }

        public void ListenToCollectorCompletelyLoaded(UnityAction listener)
        {
            CollectorCompletelyLoaded.ListenToUnityEvent(listener);
        }

        public void StopListeningToCollectorCompletelyLoaded(UnityAction listener)
        {
            CollectorCompletelyLoaded.StopListeningToUnityEvent(listener);
        }


        private UnityEvent<Storage> ContactWithStorageHappened { get; set; }

        public void ListenToContactWithStorageHappened(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.AddListener(listener);
        }

        public void StopListeningToContactWithStorageHappened(UnityAction<Storage> listener)
        {
            ContactWithStorageHappened.RemoveListener(listener);
        }

        #endregion

        public void Init
        (
            Collider2D pheromone_detection_area,
            Rigidbody2D rigidbody,
            Transform ant,
            Detector pheromone_detector,
            UnityEvent<Collectable>? contact_with_non_empty_collectable_happened,
            UnityEventSubscription collector_completely_emptied,
            UnityEventSubscription collector_completely_loaded,
            UnityEvent<Storage> contact_with_storage_happened
        )
        {
            shared_info.Init(rigidbody, ant);
            ContactWithNonEmptyCollectableHappened = contact_with_non_empty_collectable_happened;
            SeekTacticianInfo.Init(shared_info, pheromone_detection_area, pheromone_detector);
            CollectorCompletelyEmptied = collector_completely_emptied;
            CollectorCompletelyLoaded = collector_completely_loaded;
            ContactWithStorageHappened = contact_with_storage_happened;
        }
    }
}