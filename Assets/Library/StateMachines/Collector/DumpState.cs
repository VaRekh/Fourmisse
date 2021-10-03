using Assets.Scripts;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector
{
    public class DumpState : State<StateCode, CollectorInfo>
    {
        private Anthill anthill;
        
        public DumpState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Dump);

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);
            anthill = null;
            anthill = GetAnthill(data[0]);

            static Anthill GetAnthill(object data)
            {
                Anthill anthill_found = data as Anthill;
                Assert.IsNotNull(anthill_found);

                return anthill_found;
            }
        }

        public override void Update(float delta_time)
        {
            anthill.Store(Info.Load.CurrentValue);
            Info.Load.CurrentValue = 0U;

            Updater.Change(StateCode.Idle);
        }
    }
}
