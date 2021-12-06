using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Collector
{
    public class IdleState : State<StateCode, CollectorInfo>
    {
        public IdleState(Updater<StateCode, CollectorInfo> updater, CollectorInfo info)
            : base(updater, info)
        { }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Idle);
        }

        public override void OnTriggerEnter2D(Collider2D collision, params object[] data)
        {
            Assert.IsTrue(data.Length == 1);

            switch(data[0])
            {
                case Collectable collectable:
                    Updater.Change(StateCode.Collect, collectable);
                    break;
                case Storage storage:
                    Updater.Change(StateCode.Dump, storage);
                    break;
                default:
                    break;
            }
        }
    }
}