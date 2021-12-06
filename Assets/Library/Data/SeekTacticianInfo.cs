using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Assets.Library.Data
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
        private Detector pheromone_detector;

        private readonly HashSet<Entity> tracked_pheromones = new HashSet<Entity>();

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

        private  Detector PheromoneDetector
        {
            set
            {
                Assert.IsNotNull(value);
                pheromone_detector = value;
            }
        }

        public void Init(ControllerInfo.SharedInfo shared_info, Collider2D pheromone_detection_area, Detector pheromone_detector)
        {
            SharedInfo = shared_info;
            Collider = pheromone_detection_area;
            PheromoneDetector = pheromone_detector;
        }

        public HashSet<Entity> TrackedPheromones
            => tracked_pheromones;

        public UnityEvent<Entity, List<Entity>> PheromoneAppeared
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.EntityAppeared;
            }
        }

        public UnityEvent<Entity, List<Entity>> PheromoneVanished
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.EntityVanished;
            }
        }

        public List<Entity> DetectedPheromones
        {
            get
            {
                Assert.IsNotNull(pheromone_detector);
                return pheromone_detector.DetectedEntitiesAsList;
            }
        }

        public void AddToTrackedPheromones(Entity pheromone)
            => TrackedPheromones.Add(pheromone);

        public void ClearTrackedPheromones()
            => TrackedPheromones.Clear();

        public Entity GetNearestPheromone(List<Entity> pheromones)
        {
            Vector2 ant_collider_position = Collider.transform.position;
            Entity nearest_pheromone = pheromone_detector.GetNearestEntity(pheromones, ant_collider_position);
            return nearest_pheromone;
        }

        public List<Entity> GetUntrackedPheromones(List<Entity> pheromones, HashSet<Entity> tracked_pheromones)
        {
            List<Entity> untracked_pheromones = pheromone_detector.GetUntrackedEntities(pheromones, tracked_pheromones);
            return untracked_pheromones;
        }
    }
}
