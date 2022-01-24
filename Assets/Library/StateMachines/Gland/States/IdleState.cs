namespace Assets.Library.StateMachines.Gland.States
{
    public class IdleState : State<StateCode, Info>
    {
        public IdleState(Updater<StateCode, Info> updater, Info info)
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
