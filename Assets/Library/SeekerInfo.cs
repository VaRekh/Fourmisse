using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;
using CollectorStateCode = Assets.Library.StateMachines.Collector.StateCode;

namespace Assets.Library
{
    [Serializable]
    public class SeekerInfo
    {
        [SerializeField]
        private UnityEvent<CollectorStateCode> collector_state_changed;

        public UnityEvent<CollectorStateCode> CollectorStateChanged
        {
            get => collector_state_changed;
            set => collector_state_changed = value;
        }
    }
}
