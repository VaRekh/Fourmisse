using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library.Data;
using Assets.Library.StateMachines;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using SeekerFactory = Assets.Library.StateMachines.Seeker.Factory;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Seeker : MonoBehaviour
    {
        [SerializeField]
        private SeekerInfo info;

        private Updater<SeekerStateCode, SeekerInfo> seeker_updater;

        public Updater<SeekerStateCode, SeekerInfo> SeekerUpdater
            => seeker_updater;

        public SeekerStateCode CurrentState
            => seeker_updater.CurrentStateCode;

        public UnityEvent<SeekerStateCode> StateChanged
            => seeker_updater.StateChanged;

        private void Start()
        {
            var parent_transform = transform.parent;
            Assert.IsNotNull(parent_transform);

            var collectors = parent_transform.GetComponentsInChildren<Collector>();
            Assert.IsTrue(collectors.Length == 1);
            info.CollectorStateChanged = collectors[0].StateChanged;

            SeekerFactory seeker_factory = new SeekerFactory();
            seeker_updater = new Updater<SeekerStateCode, SeekerInfo>(info, seeker_factory);

            seeker_updater.Start();
        }

        private void Update()
        {
            seeker_updater.Update(Time.deltaTime);
        }
    }
}
