using UnityEngine;

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
            Info.ListenToContactWithStorageHappened(OnContactWithStorageHappened);
            Info.ListenToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);

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
            Info.StopListeningToContactWithStorageHappened(OnContactWithStorageHappened);
            Info.StopListeningToContactWithNonEmptyCollectableHappened(OnContactWithNonEmptyCollectableHappened);
        }

        public void OnContactWithStorageHappened(Storage storage)
        {
            Updater.Change(StateCode.Idle);
        }

        public void OnContactWithNonEmptyCollectableHappened(Collectable collectable)
        {
            if (Info.CollectorIsNotFull)
            {
                Updater.Change(StateCode.Idle);
            }
        }
    }
}