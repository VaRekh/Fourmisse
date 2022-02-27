#nullable enable
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

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
            private Transform ant;
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
            private Rigidbody2D rigidbody = new(); // TODO: need refactoring because assigning a new() to this field is useless as it is later replaced by a real object


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
        private SharedInfo shared_info = new();

        [SerializeField]
        private readonly SeekTacticianInfo seek_tactician_info = new();
        [SerializeField]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        private Transform anthill;
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

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

        private BoundedUint Load { get; set; } = new(); // TODO: need refactoring because assigning a new() to this field is useless as it is later replaced by a real object


        public bool CollectorIsNotFull
            => Load.IsNotFull;

        #region Subscribe/Unsubscribed
        private UnityEvent<Collectable> ContactWithNonEmptyCollectableHappened { get; set; } = new(); // TODO: need refactoring because assigning a new() to this field is useless as it is later replaced by a real object

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


        private UnityEvent<Storage> ContactWithStorageHappened { get; set; } = new(); // TODO: need refactoring because assigning a new() to this field is useless as it is later replaced by a real object

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
            Detector<PheromoneInfo> pheromone_detector,
            UnityEvent<Collectable> contact_with_non_empty_collectable_happened,
            UnityEventSubscription collector_completely_emptied,
            UnityEventSubscription collector_completely_loaded,
            UnityEvent<Storage> contact_with_storage_happened,
            BoundedUint load
        )
        {
            shared_info.Init(rigidbody, ant);
            ContactWithNonEmptyCollectableHappened = contact_with_non_empty_collectable_happened;
            SeekTacticianInfo.Init(shared_info, pheromone_detection_area, pheromone_detector);
            CollectorCompletelyEmptied = collector_completely_emptied;
            CollectorCompletelyLoaded = collector_completely_loaded;
            ContactWithStorageHappened = contact_with_storage_happened;
            Load = load;
        }
    }
}