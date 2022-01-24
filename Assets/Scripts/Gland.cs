using UnityEngine;
using UnityEngine.Assertions;
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
        private SerializedInfo info;

        private Updater<GlandStateCode, Info> gland_updater;

        private void Start()
        {
            var collectable_detector = transform.parent.GetComponentInChildren<CollectableDetector>();
            Assert.IsNotNull(collectable_detector);

            var storage_detector = transform.parent.GetComponentInChildren<StorageDetector>();
            Assert.IsNotNull(storage_detector);

            Info actual_info = info.Build
            (
                collectable_detector.ContactWithCollectableLost,
                storage_detector.ContactWithStorageHappened
            );

            GlandFactory gland_factory = new GlandFactory();

            gland_updater = new Updater<GlandStateCode, Info>(actual_info, gland_factory);
            gland_updater.Start();
        }

        private void Update()
        {
            gland_updater.Update(Time.deltaTime);
        }
    }
}