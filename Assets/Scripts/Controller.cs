using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library;
using Assets.Library.StateMachines.Controller;
using Assets.Library.StateMachines;
using ControllerStateCode = Assets.Library.StateMachines.Controller.StateCode;
using ControllerFactory = Assets.Library.StateMachines.Controller.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private ControllerInfo info;
        [SerializeField]
        private LayerReference ant_pheromone_detection_layer;

        private Updater<ControllerStateCode, ControllerInfo> controller_updater;

        public Transform Anthill
        {
            set
            {
                info.Anthill = value;
            }
        }


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(ant_pheromone_detection_layer);

            var parent_transform = transform.parent;
            Assert.IsNotNull(parent_transform);

            var pheromone_detector = RetrieveComponentsInChildren<PheromoneDetector>(parent_transform);
            var detector = pheromone_detector.Detector;
            Assert.IsNotNull(detector);

            var rb = GetComponentInParent<Rigidbody2D>();
            Assert.IsNotNull(rb);

            var candidate_colliders = parent_transform.GetComponentsInChildren<Collider2D>();

            Collider2D pheromone_detection_area = null;

            foreach (Collider2D candidate_collider in candidate_colliders)
            {
                if (candidate_collider.gameObject.layer == ant_pheromone_detection_layer.Index)
                {
                    pheromone_detection_area = candidate_collider;
                    break;
                }
            }
            Assert.IsNotNull(pheromone_detection_area);


            var collectable_detector = RetrieveComponentsInChildren<CollectableDetector>(parent_transform);

            var collector = RetrieveComponentsInChildren<Collector>(parent_transform);

            var collector_completely_emptied_event = new UnityEventSubscription
            {
                ListenToUnityEvent = collector.ListenToCollectorCompletelyEmptied,
                StopListeningToUnityEvent = collector.StopListeningToCollectorCompletelyEmptied
            };

            var collector_completely_loaded_event = new UnityEventSubscription
            {
                ListenToUnityEvent = collector.ListenToCollectorCompletelyLoaded,
                StopListeningToUnityEvent = collector.StopListeningToCollectorCompletelyLoaded
            };

            var storage_detector = RetrieveComponentsInChildren<StorageDetector>(parent_transform);

            info.Init
            (
                pheromone_detection_area,
                rb,
                parent_transform,
                detector,
                collectable_detector.ContactWithNonEmptyCollectableHappened,
                collector_completely_emptied_event,
                collector_completely_loaded_event,
                storage_detector.ContactWithStorageHappened
            );
            

            ControllerFactory controller_factory = new ControllerFactory();
            controller_updater = new Updater<ControllerStateCode, ControllerInfo>(info, controller_factory, ControllerStateCode.Seek);
            controller_updater.Start();

            static T RetrieveComponentsInChildren<T>(Transform parent_transform)
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
            controller_updater.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            controller_updater.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}