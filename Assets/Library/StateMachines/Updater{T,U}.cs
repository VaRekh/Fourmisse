using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library.StateMachines
{
    public class Updater<TStateCode, TInfo>
        where TStateCode : Enum
    {
        private State<TStateCode, TInfo>current_state;

        private readonly State<TStateCode, TInfo>[] states;

        public TStateCode CurrentStateCode { get; private set; }

        public UnityEvent<TStateCode> StateChanged { get; }

        public Updater
        (
            TInfo info,
            IFactory<TStateCode, TInfo> factory,
            TStateCode initial_state = default
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

            CurrentStateCode = initial_state;
            int current_state_index = Enum<TStateCode>.Convert(CurrentStateCode);
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

            CurrentStateCode = new_state_code;
            current_state = RetrieveState(CurrentStateCode);

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

        public void OnTriggerEnter2D(Collider2D collision, params object[] data)
        {
            current_state.OnTriggerEnter2D(collision, data);
        }

        public void OnTriggerExit2D(Collider2D collision, params object[] data)
        {
            current_state.OnTriggerExit2D(collision, data);
        }
    }
}
