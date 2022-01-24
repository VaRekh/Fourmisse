using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Library.StateMachines.Collector.States
{
    public class IdleState : State<StateCode, CollectorInfo>
    {
        public IdleState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
            Info.ListenToContactWithNonEmptyCollectableHappenned(OnContactWithNonEmptyCollectableHappenned);
            Info.ListenToContactWithStorageHappened(OnContactWithStorageHappened);
        }

        public override void Exit()
        {
            Info.StopListeningToContactWithNonEmptyCollectableHappenned(OnContactWithNonEmptyCollectableHappenned);
            Info.StopListeningToContactWithStorageHappened(OnContactWithStorageHappened);
        }

        public void OnContactWithNonEmptyCollectableHappenned(Collectable collectable)
        {
            Updater.Change(StateCode.Collect, collectable);
        }

        public void OnContactWithStorageHappened(Storage storage)
        {
            Updater.Change(StateCode.Dump, storage);
        }
    }
}