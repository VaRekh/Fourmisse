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
        [SerializeField]
        private float movespeed;
        [SerializeField]
        private SeekTacticianInfo seek_tactician_info;
        [SerializeField]
        private Transform anthill;
        [SerializeField][HideInInspector]
        private Transform ant;
        [SerializeField][HideInInspector]
        private Rigidbody2D rigidbody;
        [SerializeField][HideInInspector]
        private UnityEvent<SeekerStateCode> seeker_state_changed;
        [SerializeField][HideInInspector]
        private PheromoneDetector pheromone_detector;

        public float Movespeed
            => movespeed;

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

        public Transform Ant
        {
            set
            {
                Assert.IsNotNull(value);
                ant = value;
                SeekTacticianInfo.Ant = value;
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
            set
            {
                Assert.IsNotNull(value);
                rigidbody = value;
                SeekTacticianInfo.Rigidbody = value;
            }
        }

        public UnityEvent<SeekerStateCode> SeekerStateChanged
        {
            get => seeker_state_changed;
            set
            {
                Assert.IsNotNull(value);
                seeker_state_changed = value;
            }
        }

        public PheromoneDetector PheromoneDetector
        {
            set => SeekTacticianInfo.PheromoneDetector = value;
        }

        public Collider2D Collider
        {
            set => SeekTacticianInfo.Collider = value;
        }
    }
}