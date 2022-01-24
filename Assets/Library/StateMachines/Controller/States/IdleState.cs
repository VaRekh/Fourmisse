#nullable enable
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Controller.States
{
    public class IdleState : State<StateCode, ControllerInfo>
    {
        private Collectable? CurrentlyCollectedCollectable { get; set; }

        public IdleState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
            Info.ListenToCollectorCompletelyEmptied(OnCollectorCompletelyEmptied);
            Info.ListenToCollectorCompletelyLoaded(OnCollectorCompletelyLoaded);

            Assert.IsTrue(data.Length == 0 || data.Length == 1);

            if (data.Length == 1)
            {
                var collectable = data[0] as Collectable;
                Assert.IsNotNull(collectable);
                CurrentlyCollectedCollectable = collectable;
            }
            
            CurrentlyCollectedCollectable?.ListenToCollectableCompletelyEmptied(OnCollectableCompletelyEmptied);

            Info.Rigidbody.ChangeDirection(0f, Vector2.zero);
        }

        public override void Exit()
        {
            Info.StopListeningToCollectorCompletelyEmptied(OnCollectorCompletelyEmptied);
            Info.StopListeningToCollectorCompletelyLoaded(OnCollectorCompletelyLoaded);

            CurrentlyCollectedCollectable?.StopListeningToCollectableCompletelyEmptied(OnCollectableCompletelyEmptied);
            CurrentlyCollectedCollectable = null;
        }

        public void OnCollectorCompletelyEmptied()
        {
            Updater.Change(StateCode.Seek);
        }

        public void OnCollectorCompletelyLoaded()
        {
            Updater.Change(StateCode.Return);
        }

        public void OnCollectableCompletelyEmptied()
        {
            Updater.Change(StateCode.Return);
        }
    }
}