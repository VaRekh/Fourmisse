using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library.StateMachines
{
    public class Updater<TStateCode, TInfo>
        where TStateCode : Enum
    {
        private TStateCode current_state_code;
        private State<TStateCode, TInfo>current_state;

        private readonly State<TStateCode, TInfo>[] states;

        public UnityEvent<TStateCode> StateChanged;

        public Updater
        (
            TInfo info,
            IFactory<TStateCode, TInfo> factory
        )
        {
            var state_codes = Enum<TStateCode>.Values;

            int states_count = Enum<TStateCode>.Count;
            states = new State<TStateCode, TInfo>[states_count];

            foreach (TStateCode code in state_codes)
            {
                State<TStateCode, TInfo> state = factory.Get(code, this, info);
                int state_index = Enum<TStateCode>.Convert(code);
                states[state_index] = state;
            }

            current_state_code = default;
            int current_state_index = Enum<TStateCode>.Convert(current_state_code);
            current_state = states[current_state_index];

            StateChanged = new UnityEvent<TStateCode>();
        }

        public void Start()
        {
            current_state.Enter();
        }

        public void Change(TStateCode new_state_code, params object[] data)
        {
            current_state.Exit();

            current_state_code = new_state_code;
            current_state = RetrieveState(current_state_code);

            current_state.Enter(data);

            State<TStateCode, TInfo> RetrieveState(TStateCode code)
            {
                int state_index = Enum<TStateCode>.Convert(code);
                State<TStateCode, TInfo> state = states[state_index];
                return state;
            }
        }

        public void FixedUpdate(float delta_time)
        {
            current_state.FixedUpdate(delta_time);
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
