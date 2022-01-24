using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector.States
{
    public class DumpState : State<StateCode, CollectorInfo>
    {
        private Storage storage;
        
        public DumpState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Dump);

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);
            storage = null;
            storage = GetStorage(data[0]);

            static Storage GetStorage(object data)
            {
                Storage storage_found = data as Storage;
                Assert.IsNotNull(storage_found);

                return storage_found;
            }
        }

        public override void Update(float delta_time)
        {
            storage.Store(Info.Load.CurrentValue);
            Info.Load.CurrentValue = 0U;

            Updater.Change(StateCode.Idle);
        }
    }
}
