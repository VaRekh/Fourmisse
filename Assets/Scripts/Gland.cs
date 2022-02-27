#nullable enable
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library;
using Assets.Library.StateMachines;
using Assets.Library.StateMachines.Gland;
using GlandStateCode = Assets.Library.StateMachines.Gland.StateCode;
using GlandFactory = Assets.Library.StateMachines.Gland.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Gland : MonoBehaviour
    {
        [SerializeField]
        private SerializedInfo info = new();

        private Updater<GlandStateCode, Info>? gland_updater;

        private void Start()
        {
            var transform_parent = transform.parent;

            var collectable_detector = transform_parent.GetComponentInChildren<CollectableDetector>();
            Assert.IsNotNull(collectable_detector);

            var storage_detector = transform_parent.GetComponentInChildren<StorageDetector>();
            Assert.IsNotNull(storage_detector);

            var collector = transform_parent.GetComponentInChildren<Collector>();
            Assert.IsNotNull(collector);

            Info actual_info = info.Build
            (
                new NonSerializedInfo
                {
                    ContactWithCollectableLost = collectable_detector.ContactWithCollectableLost,
                    ContactWithStorageHappened = storage_detector.ContactWithStorageHappened,
                    ContactWithNonEmptyCollectableHappened = collectable_detector.ContactWithNonEmptyCollectableHappened,
                    Load = collector.Load,
                    InitPheromone = InitPheromone,
                    Identifier = new Identifier(transform_parent.gameObject.GetInstanceID())
                }
            );

            var gland_factory = new GlandFactory();

            gland_updater = new Updater<GlandStateCode, Info>(actual_info, gland_factory);
            gland_updater.Start();
        }

        private void Update()
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            gland_updater.Update(Time.deltaTime);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        private void InitPheromone(GameObject pheromone, Identifier ant_identifier, uint intensity)
        {
            pheromone.GetComponent<Pheromone>().Init(ant_identifier, intensity);
        }
    }
}