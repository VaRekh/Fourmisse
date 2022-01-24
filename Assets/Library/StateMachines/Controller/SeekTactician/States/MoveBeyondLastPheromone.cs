using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Assets.Library.StateMachines.Controller.SeekTactician.States
{
    public class MoveBeyondLastPheromone : State<StateCode, SeekTacticianInfo>
    {
        private Vector2 last_pheromone;
        private bool last_pheromone_reached;
        private readonly Stopwatch stopwatch;

        public MoveBeyondLastPheromone(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
            : base(updater, info)
        {
            stopwatch = new Stopwatch();
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.MoveBeyondLastPheromone);
            Info.PheromoneAppeared.AddListener(ReactToPheromoneAppeared);

            bool is_data_available = data.Length > 0;
            Assert.IsTrue(is_data_available);
            last_pheromone = (Vector3)data[0];

            last_pheromone_reached = false;
            stopwatch.Reset();
        }

        public override void Update(float delta_time)
        {
            if (!last_pheromone_reached)
            {
                float distance_to_pheromone = Info.AntPosition.DistanceTo(last_pheromone);
                bool is_close_to_pheromone = distance_to_pheromone <= Info.CloseRangeFromPheromone;

                if (is_close_to_pheromone)
                {
                    last_pheromone_reached = true;
                }
            }
            else
            {
                stopwatch.Update(delta_time);

                if (stopwatch.CurrentValue >= Info.SecondsBeforeSwitchingToRandomMove)
                {
                    Updater.Change(StateCode.RandomMove);
                }
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