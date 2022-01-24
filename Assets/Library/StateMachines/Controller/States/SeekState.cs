using TacticianStateCode = Assets.Library.StateMachines.Controller.SeekTactician.StateCode;
using TacticianFactory = Assets.Library.StateMachines.Controller.SeekTactician.Factory;
using Assets.Library.StateMachines.Controller.SeekTactician;

namespace Assets.Library.StateMachines.Controller.States
{
    public class SeekState : State<StateCode, ControllerInfo>
    {
        private readonly Updater<TacticianStateCode, SeekTacticianInfo> seek_tactician_updater;

        public SeekState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        {
            TacticianFactory seek_tactician_factory = new TacticianFactory();
            seek_tactician_updater = new Updater<TacticianStateCode, SeekTacticianInfo>(Info.SeekTacticianInfo, seek_tactician_factory);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Seek);
            Info.ListenToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);
        }

        public override void Update(float delta_time)
        {
            seek_tactician_updater.Update(delta_time);
        }

        public override void FixedUpdate(float delta_time)
        {
            seek_tactician_updater.FixedUpdate(delta_time);
        }

        public override void Exit()
        {
            seek_tactician_updater.Change(TacticianStateCode.Idle);
            Info.StopListeningToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);
        }

        private void OnContactWithNonEmptyCollectableHappened(Collectable collectable)
        {
            Updater.Change(StateCode.Idle, collectable);
        }
    }
}