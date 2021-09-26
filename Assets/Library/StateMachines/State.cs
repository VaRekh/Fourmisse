using UnityEngine;

namespace Assets.Library.StateMachines
{
    public abstract class State<StateUpdater>
    {
        protected StateUpdater state_updater;

        public State(StateUpdater updater)
        {
            state_updater = updater;
        }


        public virtual void Enter(params object[] data)
        {

        }

        public virtual void Update(float delta_time)
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {

        }
    }
}
