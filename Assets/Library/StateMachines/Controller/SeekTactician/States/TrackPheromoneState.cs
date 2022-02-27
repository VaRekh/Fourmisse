#nullable enable
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Assets.Library.StateMachines.Controller.SeekTactician.States
{
    public class TrackPheromoneState : State<StateCode, SeekTacticianInfo>
    {
        private PheromoneInfo pheromone_to_track;
        private SeekTacticianInfo.ChangeDirectionData change_direction_data;

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public TrackPheromoneState(Updater<StateCode, SeekTacticianInfo> updater, SeekTacticianInfo info)
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
            : base(updater, info)
        {
        }

        public override void Enter(params object[] data)
        {
            Updater.StateChanged.Invoke(StateCode.TrackPheromone);
            Assert.IsTrue(data.Length > 0);
            var pheromone = data[0] as PheromoneInfo;
            Assert.IsNotNull(pheromone);

#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            UpdateTrackingTo(pheromone);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
            Info.PheromoneVanished.AddListener(ReactToPheromoneVanished);
        }

        public override void Update(float delta_time)
        {
            float distance_to_pheromone = Info.AntPosition.DistanceTo(pheromone_to_track.Position);
            bool is_close_to_pheromone = distance_to_pheromone <= Info.CloseRangeFromPheromone;

            if (is_close_to_pheromone)
            {
                List<PheromoneInfo> pheromones = Info.DetectedPheromones;
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

        private void ReactToPheromoneVanished(PheromoneInfo pheromone, List<PheromoneInfo> pheromones)
        {
            if (pheromone == pheromone_to_track)
            {
                ReactToPheromoneToTrackUpdates(pheromones);
            }
        }

        private void UpdateTrackingTo(PheromoneInfo pheromone)
        {
            Assert.IsNotNull(pheromone);
            bool is_tracked_pheromone = Info.TrackedPheromones.Contains(pheromone);
            Assert.IsFalse(is_tracked_pheromone);

            Vector2 normalized_direction = Info.AntPosition.NormalizedDirectionTo(pheromone.Position);
            change_direction_data = new SeekTacticianInfo.ChangeDirectionData(normalized_direction);
            pheromone_to_track = pheromone;
            Info.AddToTrackedPheromones(pheromone_to_track);
        }

        private void ReactToPheromoneToTrackUpdates(List<PheromoneInfo> pheromones)
        {
            List<PheromoneInfo> untracked_pheromones = Info.GetUntrackedPheromones(pheromones, Info.TrackedPheromones);
            untracked_pheromones.Remove(pheromone_to_track);
            PheromoneInfo? pheromone = Info.GetNearestPheromone(untracked_pheromones);

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