using Assets.Scripts;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector
{
    public class DumpState : State
    {
        private Anthill anthill;
        
        public DumpState(StateUpdater updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            state_updater.StateChanged.Invoke(StateCode.Dump);

            anthill = GetAnthill(data);

            Anthill GetAnthill(object[] data)
            {
                bool is_data_available = data.Length > 0;
                Anthill anthill_found = is_data_available ? data[0] as Anthill : null;

                Assert.IsNotNull(anthill_found);

                return anthill_found;
            }
        }

        public override void Update(float delta_time)
        {
            anthill.Store(Info.Load.CurrentValue);
            Info.Load.CurrentValue = 0U;

            state_updater.Change(StateCode.Idle);
        }
    }
}
