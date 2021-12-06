using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Controller.SeekTactician
{
    public class RandomMoveState : State<StateCode, SeekTacticianInfo>
    {
        private readonly Stopwatch random_move_stopwatch;
        private readonly Vector2Generator vector_generator;
        private SeekTacticianInfo.ChangeDirectionData change_direction_data;

        public RandomMoveState(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
            : base(updater, info)
        {
            random_move_stopwatch = new Stopwatch(Info.DirectionChangeInterval);

            Range range = new Range(-1f, 1f);
            vector_generator = new Vector2Generator(range, range);
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.RandomMove);
            Info.PheromoneAppeared.AddListener(ReactToPheromoneAppeared);

            random_move_stopwatch.Reset(Info.DirectionChangeInterval);
        }

        public override void Update(float delta_time)
        {
            random_move_stopwatch.Update(delta_time);

            if (random_move_stopwatch.CurrentValue >= Info.DirectionChangeInterval)
            {
                Vector2 random_normalized_direction = vector_generator.GetNormalized();
                change_direction_data = new SeekTacticianInfo.ChangeDirectionData(random_normalized_direction);

                random_move_stopwatch.Reset();
            }
        }

        public override void FixedUpdate(float delta_time)
        {
            if (change_direction_data.Enabled)
            {
                Info.Rigidbody.ChangeDirection(Info.Movespeed, change_direction_data.Direction);
                change_direction_data.Enabled = false;
            }
        }

        public override void Exit()
        {
            Info.PheromoneAppeared.RemoveListener(ReactToPheromoneAppeared);
        }

        private void ReactToPheromoneAppeared(Entity pheromone, List<Entity> pheromones)
        {
            bool has_pheromone_been_tracked = Info.TrackedPheromones.Contains(pheromone);
            if (!has_pheromone_been_tracked)
            {
                Updater.Change(StateCode.TrackPheromone, pheromone);
            }
        }
    }
}