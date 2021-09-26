using System;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts;

namespace Assets.Library.StateMachines.Collector
{
    public class StateUpdater
    {
        private StateCode current_state_code;
        private State current_state;

        private State[] states;

        public UnityEvent<StateCode> StateChanged;


        public StateUpdater(CollectorInfo info, StateCode base_state_code = StateCode.Idle)
        {
            var state_codes = Enum.GetValues(typeof(StateCode));

            states = new State[state_codes.Length];

            foreach (StateCode code in state_codes)
            {
                State state = null;

                switch (code)
                {
                    case StateCode.Idle:
                        state = new IdleState(this, info);
                        break;
                    case StateCode.Collect:
                        state = new CollectState(this, info);
                        break;
                    case StateCode.Dump:
                        state = new DumpState(this, info);
                        break;
                    default:
                        break;
                }

                states[(int)code] = state;
            }


            current_state_code = base_state_code;
            current_state = states[(int)current_state_code];

            StateChanged = new UnityEvent<StateCode>();
        }



        public void Start()
        {
            current_state.Enter();
        }

        public void Change(StateCode new_state_code, params object[] data)
        {
            current_state.Exit();

            current_state_code = new_state_code;
            current_state = states[(int)current_state_code];

            current_state.Enter(data);
        }

        public void Update(float delta_time)
        {
            current_state.Update(delta_time);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            current_state.OnTriggerEnter2D(collision);
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            current_state.OnTriggerExit2D(collision);
        }
    }
}
