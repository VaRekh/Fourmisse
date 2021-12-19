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
            Info.ListenToLossOfContactWithCollectable(OnLossOfContact);
        }

        public override void Exit()
        {
            Info.StopListeningToLossOfContactWithCollectable(OnLossOfContact);
        }

        private void OnLossOfContact(Collectable collectable)
        {
            Updater.Change(StateCode.Production);
        }
    }
}
