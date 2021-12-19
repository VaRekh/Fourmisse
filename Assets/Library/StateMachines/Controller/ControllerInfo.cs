using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library.Data;
using Assets.Library.StateMachines.Controller.SeekTactician;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library.StateMachines.Controller
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
        private Detector pheromone_detector;

        private Updater<SeekerStateCode, SeekerInfo> seeker;

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

        private Updater<SeekerStateCode, SeekerInfo> Seeker
        {
            set
            {
                Assert.IsNotNull(value);
                seeker = value;
            }
        }

        public SeekerStateCode SeekerCurrentState
            => seeker.CurrentStateCode;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
            => seeker.StateChanged;

        public void Init
        (
            Collider2D pheromone_detection_area,
            Rigidbody2D rigidbody,
            Transform ant,
            Updater<SeekerStateCode, SeekerInfo> seeker,
            Detector pheromone_detector
        )
        {
            Seeker = seeker;
            shared_info.Init(rigidbody, ant);
            SeekTacticianInfo.Init(shared_info, pheromone_detection_area, pheromone_detector);
        }
    }
}