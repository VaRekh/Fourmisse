using UnityEngine;

using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.StateMachines.Controller
{
    public class IdleState : State<StateCode, ControllerInfo>
    {
        public IdleState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);

            Info.Rigidbody.ChangeDirection(0f, Vector2.zero);
        }

        public override void Exit()
        {
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        public void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Seek:
                    Updater.Change(StateCode.Seek);
                    break;
                case SeekerStateCode.Return:
                    Updater.Change(StateCode.Return);
                    break;
                default:
                    break;
            }
        }
    }
}