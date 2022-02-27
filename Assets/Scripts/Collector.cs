#nullable enable
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library;
using Assets.Library.StateMachines;
using Assets.Library.StateMachines.Collector;
using CollectorFactory = Assets.Library.StateMachines.Collector.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Collector : MonoBehaviour
    {
        [SerializeField]
        private CollectorInfo info = new();

        public BoundedUint Load
            => info.Load;

        public void ListenToCollectorCompletelyEmptied(UnityAction listener)
            => info.ListenToCollectorCompletelyEmptied(listener);
        public void StopListeningToCollectorCompletelyEmptied(UnityAction listener)
            => info.StopListeningToCollectorCompletelyEmptied(listener);

        public void ListenToCollectorCompletelyLoaded(UnityAction listener)
            => info.ListenToCollectorCompletelyLoaded(listener);
        public void StopListeningToCollectorCompletelyLoaded(UnityAction listener)
            => info.StopListeningToCollectorCompletelyLoaded(listener);


        private Updater<StateCode, CollectorInfo>? collect_updater;



        // Start is called before the first frame update
        private void Start()
        {
            var collector_factory = new CollectorFactory();


            var parent_transform = transform.parent;
            Assert.IsNotNull(parent_transform);


            var collectable_detector = GetComponentsInChildren<CollectableDetector>(parent_transform);

            var storage_detector = GetComponentsInChildren<StorageDetector>(parent_transform);

            info.Build
            (
                collectable_detector.ContactWithNonEmptyCollectableHappened,
                storage_detector.ContactWithStorageHappened
            );

            collect_updater = new Updater<StateCode, CollectorInfo>(info, collector_factory);
            collect_updater.Start();


            static T GetComponentsInChildren<T>(Transform parent_transform)
                where T : MonoBehaviour
            {
                var components = parent_transform.GetComponentsInChildren<T>();
                Assert.IsTrue(components.Length == 1);
                var component = components[0];

                return component;
            }
        }

        // Update is called once per frame
        private void Update()
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            collect_updater.Update(Time.deltaTime);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }
    }
}