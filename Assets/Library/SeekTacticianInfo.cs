using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Scripts;
using System.Collections.Generic;

namespace Assets.Library
{
    [Serializable]
    public class SeekTacticianInfo
    {
        public struct ChangeDirectionData
        {
            public bool Enabled { get; set; }
            public Vector2 Direction { get; private set; }
            public ChangeDirectionData(Vector2 direction)
            {
                Enabled = true;
                Direction = direction;
            }
        }


        private ControllerInfo.SharedInfo shared_info;

        [SerializeField]
        [Tooltip(
        "Distance from a pheromone which is considered close enough " +
        "to look for a new pheromone"
        )]
        private float close_range_from_pheromone;
        [SerializeField]
        private float direction_change_per_second;
        [SerializeField]
        private float seconds_before_switching_to_random_move;
        [SerializeField][HideInInspector]
        private Collider2D collider;
        [SerializeField][HideInInspector]
        private PheromoneDetector pheromone_detector;

        private readonly HashSet<Pheromone> tracked_pheromones = new HashSet<Pheromone>();

        private ControllerInfo.SharedInfo SharedInfo
        {
            set
            {
                Assert.IsNotNull(value);
                shared_info = value;
            }
        }

        public float CloseRangeFromPheromone
            => close_range_from_pheromone;

        public float DirectionChangeInterval
            => 1f / direction_change_per_second;

        public float SecondsBeforeSwitchingToRandomMove
            => seconds_before_switching_to_random_move;

        public float Movespeed
        {
            get
            {
                Assert.IsNotNull(shared_info);
                return shared_info.Movespeed;
            }
        }

        public Vector2 AntPosition
            => shared_info.AntPosition;

        public Collider2D Collider
        {
            get => collider;
            private set
            {
                Assert.IsNotNull(value);
                collider = value;
            }
        }

        public Rigidbody2D Rigidbody
            => shared_info.Rigidbody;

        private  PheromoneDetector PheromoneDetector
        {
            set
            {
                Assert.IsNotNull(value);
                pheromone_detector = value;
            }
        }

        public void Init(ControllerInfo.SharedInfo shared_info, Collider2D pheromone_detection_area, PheromoneDetector pheromone_detector)
        {
            SharedInfo = shared_info;
            Collider = pheromone_detection_area;
            PheromoneDetector = pheromone_detector;
        }

        public HashSet<Pheromone> TrackedPheromones
            => tracked_pheromones;

        public UnityEvent<Pheromone, List<Pheromone>> PheromoneAppeared
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.PheromoneAppeared;
            }
        }

        public UnityEvent<Pheromone, List<Pheromone>> PheromoneVanished
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.PheromoneVanished;
            }
        }

        public List<Pheromone> DetectedPheromones
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.DetectedPheromonesAsList;
            }
        }

        public void AddToTrackedPheromones(Pheromone pheromone)
            => TrackedPheromones.Add(pheromone);

        public void ClearTrackedPheromones()
            => TrackedPheromones.Clear();

        public Pheromone GetNearestPheromone(List<Pheromone> pheromones)
        {
            Vector2 ant_collider_position = Collider.transform.position;
            Pheromone nearest_pheromone = pheromone_detector.GetNearestPheromone(pheromones, ant_collider_position);
            return nearest_pheromone;
        }

        public List<Pheromone> GetUntrackedPheromones(List<Pheromone> pheromones, HashSet<Pheromone> tracked_pheromones)
        {
            List<Pheromone> untracked_pheromones = pheromone_detector.GetUntrackedPheromones(pheromones, tracked_pheromones);
            return untracked_pheromones;
        }
    }
}
