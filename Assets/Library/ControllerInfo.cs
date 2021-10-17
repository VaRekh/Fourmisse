using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library
{
    [Serializable]
    public class ControllerInfo
    {
        [SerializeField]
        private float movespeed; //30
        [SerializeField]
        private float direction_change_per_second; //0.25
        [SerializeField]
        private Transform anthill;
        [SerializeField][HideInInspector]
        private Transform ant;
        [SerializeField][HideInInspector]
        private Rigidbody2D rigidbody;
        [SerializeField][HideInInspector]
        private UnityEvent<SeekerStateCode> seeker_state_changed;

        public float Movespeed
            => movespeed;

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
            set => ant = value;
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
            get
            {
                Assert.IsNotNull(rigidbody);
                return rigidbody;
            }
            set => rigidbody = value;
        }

        public float DirectionChangeInterval
            => 1f / direction_change_per_second;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
        {
            get => seeker_state_changed;
            set => seeker_state_changed = value;
        }
    }
}
