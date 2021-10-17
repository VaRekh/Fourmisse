using UnityEngine;

using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.StateMachines.Controller
{
    public class SeekState : State<StateCode, ControllerInfo>
    {
        private readonly Stopwatch stopwatch;
        private readonly Vector2Generator vector_generator;

        public SeekState(Updater<StateCode, ControllerInfo> updater, ControllerInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch(Info.DirectionChangeInterval);

            Range range = new Range(-1f, 1f);
            vector_generator = new Vector2Generator(range, range);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.Seek);
            Info.SeekerStateChanged.AddListener(ReactToSeekerStateChanged);

            stopwatch.Reset(Info.DirectionChangeInterval);
        }

        public override void Update(float delta_time)
        {
            stopwatch.Update(delta_time);

            if (stopwatch.CurrentValue >= Info.DirectionChangeInterval)
            {
                Vector2 random_normalized_direction = vector_generator.GetNormalized();
                Info.Rigidbody.ChangeDirection(Info.Movespeed, random_normalized_direction);

                stopwatch.Reset();
            }

            stopwatch.Update(Time.deltaTime);
        }

        public override void Exit()
        {
            Info.SeekerStateChanged.RemoveListener(ReactToSeekerStateChanged);
        }

        public void ReactToSeekerStateChanged(SeekerStateCode new_state)
        {
            switch (new_state)
            {
                case SeekerStateCode.Collect:
                    Updater.Change(StateCode.Idle);
                    break;
                default:
                    break;
            }
        }
    }
}