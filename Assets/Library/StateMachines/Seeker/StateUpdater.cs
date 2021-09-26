using System;
using UnityEngine;
using UnityEngine.Events;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library.StateMachines.Seeker
{
    public class StateUpdater
    {
        private StateCode current_state_code;
        private State current_state;

        private State[] states;


        public UnityEvent<StateCode> StateChanged;


        public StateUpdater
            (
            UnityEvent<CollectorStateCode> collector_state_changed,
            StateCode                      base_state_code = StateCode.Seek
            )
        {
            var state_codes = Enum.GetValues(typeof(StateCode));

            states = new State[state_codes.Length];

            foreach (StateCode code in state_codes)
            {
                State state = null;

                switch (code)
                {
                    case StateCode.Seek:
                        state = new SeekState(this);
                        break;
                    case StateCode.Collect:
                        state = new CollectState(this);
                        break;
                    case StateCode.Return:
                        state = new ReturnState(this);
                        break;
                    case StateCode.Dump:
                        state = new DumpState(this);
                        break;
                    default:
                        break;
                }

                states[(int)code] = state;
            }


            current_state_code = base_state_code;
            current_state = states[(int)current_state_code];

            StateChanged = new UnityEvent<StateCode>();

            collector_state_changed.AddListener(ReactToCollectorStateChanged);

            void ReactToCollectorStateChanged(CollectorStateCode new_state)
            {
                current_state.ReactToCollectorStateChanged(new_state);
            }
        }



        public void Start()
        {
            current_state.Enter();
        }

        public void Change(StateCode new_state_code)
        {
            current_state.Exit();

            current_state_code = new_state_code;
            current_state = states[(int)current_state_code];

            current_state.Enter();
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
