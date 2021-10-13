using System;
using UnityEngine;
using UnityEngine.Events;

using SeekerStateCode = Assets.Library.StateMachines.Seeker.StateCode;

namespace Assets.Library
{
    [Serializable]
    public class GlandInfo
    {
        [SerializeField]
        private GameObject pheromone;
        [SerializeField]
        private float pheromone_per_second;
        [SerializeField]
        private UnityEvent<SeekerStateCode> seeker_state_changed;

        private Transform generation_transform;

        public Transform GenerationTransform
        {
            get => generation_transform;
            set => generation_transform = value;
        }

        public GameObject Pheromone
            => pheromone;

        public float GenerationInterval
            => 1f / pheromone_per_second;

        public UnityEvent<SeekerStateCode> SeekerStateChanged
        {
            get => seeker_state_changed;
            set => seeker_state_changed = value;
        }
    }
}
