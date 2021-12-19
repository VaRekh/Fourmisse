using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Assets.Library.Data;

namespace Assets.Library.StateMachines.Controller.SeekTactician.States
{
    public class TrackPheromoneState : State<StateCode, SeekTacticianInfo>
    {
        private Entity pheromone_to_track;
        private SeekTacticianInfo.ChangeDirectionData change_direction_data;

        public TrackPheromoneState(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
            : base(updater, info)
        {
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.TrackPheromone);
            Assert.IsTrue(data.Length > 0);
            var pheromone = data[0] as Entity;
            Assert.IsNotNull(pheromone);

            UpdateTrackingTo(pheromone);
            Info.PheromoneVanished.AddListener(ReactToPheromoneVanished);
        }

        public override void Update(float delta_time)
        {
            float distance_to_pheromone = Info.AntPosition.DistanceTo(pheromone_to_track.Position);
            bool is_close_to_pheromone = distance_to_pheromone <= Info.CloseRangeFromPheromone;

            if (is_close_to_pheromone)
            {
                List<Entity> pheromones = Info.DetectedPheromones;
                ReactToPheromoneToTrackUpdates(pheromones);
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
            Info.PheromoneVanished.RemoveListener(ReactToPheromoneVanished);
        }

        private void ReactToPheromoneVanished(Entity pheromone, List<Entity> pheromones)
        {
            if (pheromone == pheromone_to_track)
            {
                ReactToPheromoneToTrackUpdates(pheromones);
            }
        }

        private void UpdateTrackingTo(Entity pheromone)
        {
            Assert.IsNotNull(pheromone);
            bool is_tracked_pheromone = Info.TrackedPheromones.Contains(pheromone);
            Assert.IsFalse(is_tracked_pheromone);

            Vector2 normalized_direction = Info.AntPosition.NormalizedDirectionTo(pheromone.Position);
            change_direction_data = new SeekTacticianInfo.ChangeDirectionData(normalized_direction);
            pheromone_to_track = pheromone;
            Info.AddToTrackedPheromones(pheromone_to_track);
        }

        private void ReactToPheromoneToTrackUpdates(List<Entity> pheromones)
        {
            List<Entity> untracked_pheromones = Info.GetUntrackedPheromones(pheromones, Info.TrackedPheromones);
            untracked_pheromones.Remove(pheromone_to_track);
            Entity pheromone = Info.GetNearestPheromone(untracked_pheromones);

            if (pheromone != null)
            {
                UpdateTrackingTo(pheromone);
            }
            else
            {
                Updater.Change(StateCode.MoveBeyondLastPheromone, pheromone_to_track.Position);
            }
        }
    }
}