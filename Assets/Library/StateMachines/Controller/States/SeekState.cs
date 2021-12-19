using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
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
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);
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
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        private void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Collect:
                    Updater.Change(StateCode.Idle);
                    break;
                default:
                    break;
            }
        }
    }
}