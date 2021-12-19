using UnityEngine;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Controller.States
{
    public class ReturnState : State<StateCode, ControllerInfo>
    {
        public ReturnState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Return);
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);

            Vector2 normalized_direction = ComputeNormalizedDirection(Info.AntPosition, Info.AnthillPosition);
            Info.Rigidbody.ChangeDirection(Info.Movespeed, normalized_direction);

            static Vector2 ComputeNormalizedDirection(Vector2 ant_position, Vector2 anthill_position)
            {
                Vector2 anthill_direction = ant_position.To(anthill_position);
                Vector2 normalized_direction = anthill_direction.normalized;

                return normalized_direction;
            }
        }

        public override void Exit()
        {
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        public void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Dump:
                    Updater.Change(StateCode.Idle);
                    break;
                default:
                    break;
            }
        }
    }
}