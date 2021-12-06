using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Gland
{
    public class IdleState : State<StateCode, GlandInfo>
    {
        public IdleState(Updater<StateCode, GlandInfo> updater, GlandInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);
        }

        public override void Exit()
        {
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        public void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Return:
                    Updater.Change(StateCode.Production);
                    break;
                default:
                    break;
            }
        }
    }
}
