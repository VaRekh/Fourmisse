using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Scripts;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library
{
    [Serializable]
    public class ControllerInfo
    {
        [Serializable]
        public class SharedInfo
        {
            [SerializeField]
            private float movespeed;

            private Transform ant;
            private Rigidbody2D rigidbody;


            public float Movespeed
                => movespeed;

            private Transform Ant
            {
                set
                {
                    Assert.IsNotNull(value);
                    ant = value;
                }
            }

            public Vector2 AntPosition
            {
                get
                {
                    Assert.IsNotNull(ant);
                    return ant.position;
                }
            }

            public Rigidbody2D Rigidbody
            {
                get => rigidbody;
                private set
                {
                    Assert.IsNotNull(value);
                    rigidbody = value;
                }
            }

            public void Init(Rigidbody2D rigidbody, Transform ant)
            {
                Rigidbody = rigidbody;
                Ant = ant;
            }
        }

        [SerializeField]
        private SharedInfo shared_info;

        [SerializeField]
        private SeekTacticianInfo seek_tactician_info;
        [SerializeField]
        private Transform anthill;
        [SerializeField][HideInInspector]
        private UnityEvent<SeekerStateCode> seeker_state_changed;
        [SerializeField][HideInInspector]
        private PheromoneDetector pheromone_detector;

        public float Movespeed
            => shared_info.Movespeed;

        public SeekTacticianInfo SeekTacticianInfo
            => seek_tactician_info;

        public Vector2 AnthillPosition
        {
            get
            {
                Assert.IsNotNull(anthill);
                return anthill.position;
            }
        }

        public Vector2 AntPosition
            => shared_info.AntPosition;

        public Rigidbody2D Rigidbody
            => shared_info.Rigidbody;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
        {
            get => seeker_state_changed;
            private set
            {
                Assert.IsNotNull(value);
                seeker_state_changed = value;
            }
        }

        public void Init
        (
            Collider2D pheromone_detection_area,
            Rigidbody2D rigidbody,
            Transform ant,
            UnityEvent<SeekerStateCode> seeker_state_changed,
            PheromoneDetector pheromone_detector
        )
        {
            shared_info.Init(rigidbody, ant);
            SeekTacticianInfo.Init(shared_info, pheromone_detection_area, pheromone_detector);
            SeekerStateChanged = seeker_state_changed;
        }
    }
}