using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Assets.Scripts;

namespace Assets.Library.StateMachines.Controller.SeekTactician
{
    public class TrackPheromoneState : State<StateCode, SeekTacticianInfo>
    {
        private Pheromone pheromone_to_track;
        private SeekTacticianInfo.ChangeDirectionData change_direction_data;

        public TrackPheromoneState(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
            : base(updater, info)
        {
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.TrackPheromone);
            Assert.IsTrue(data.Length > 0);
            var pheromone = data[0] as Pheromone;
            Assert.IsNotNull(pheromone);

            UpdateTrackingTo(pheromone);
            Info.PheromoneVanished.AddListener(ReactToPheromoneVanished);
        }

        public override void Update(float delta_time)
        {
            float distance_to_pheromone = Info.AntPosition.DistanceTo(pheromone_to_track.transform.position);
            bool is_close_to_pheromone = distance_to_pheromone <= Info.CloseRangeFromPheromone;

            if (is_close_to_pheromone)
            {
                List<Pheromone> pheromones = Info.DetectedPheromones;
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

        private void ReactToPheromoneVanished(Pheromone pheromone, List<Pheromone> pheromones)
        {
            if (pheromone == pheromone_to_track)
            {
                ReactToPheromoneToTrackUpdates(pheromones);
            }
        }

        private void UpdateTrackingTo(Pheromone pheromone)
        {
            Assert.IsNotNull(pheromone);
            bool is_tracked_pheromone = Info.TrackedPheromones.Contains(pheromone);
            Assert.IsFalse(is_tracked_pheromone);

            Vector2 normalized_direction = Info.AntPosition.NormalizedDirectionTo(pheromone.transform.position);
            change_direction_data = new SeekTacticianInfo.ChangeDirectionData(normalized_direction);
            pheromone_to_track = pheromone;
            Info.AddToTrackedPheromones(pheromone_to_track);
        }

        private void ReactToPheromoneToTrackUpdates(List<Pheromone> pheromones)
        {
            List<Pheromone> untracked_pheromones = Info.GetUntrackedPheromones(pheromones, Info.TrackedPheromones);
            untracked_pheromones.Remove(pheromone_to_track);
            Pheromone pheromone = Info.GetNearestPheromone(untracked_pheromones);

            if (pheromone != null)
            {
                UpdateTrackingTo(pheromone);
            }
            else
            {
                Updater.Change(StateCode.MoveBeyondLastPheromone, pheromone_to_track.transform.position);
            }
        }
    }
}