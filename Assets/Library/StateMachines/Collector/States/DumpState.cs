#nullable enable
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector.States
{
    public class DumpState : State<StateCode, CollectorInfo>
    {
        private Storage? anthill;
        
        public DumpState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Dump);

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);
            anthill = GetStorage(data[0]);

            static Storage GetStorage(object data)
            {
                Storage? storage_found = data as Storage;
                Assert.IsNotNull(storage_found);

#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
                return storage_found;
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
            }
        }

        public override void Update(float delta_time)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            anthill.Store(Info.Load.CurrentValue);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
            Info.Load.CurrentValue = 0U;

            Updater.Change(StateCode.Idle);
        }
    }
}
