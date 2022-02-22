using System;
using UnityEngine;
using UnityEngine.Events;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Anthill))]
    public class AntGenerationEnabler : MonoBehaviour
    {
        private uint load_required_for_generation;
        private UnityEvent<Action<uint>> EnableAntGeneration { get; set; }
        private Storage Storage { get; set; }

        public void Awake()
        {
            EnableAntGeneration = new();
        }

        public void Start()
        {
            Storage = GetComponent<Anthill>().Storage;
            Storage.ListenToLoadIncreased(CheckLoad);
        }

        private void CheckLoad(uint load)
        {
            bool anthill_has_enough_resources = load >= load_required_for_generation;
            if (anthill_has_enough_resources)
            {
                EnableAntGeneration.Invoke(Storage.Consume);
            }
        }

        public void ListenToEnableAntGeneration(UnityAction<Action<uint>> listener, uint load_required)
        {
            load_required_for_generation = load_required;
            EnableAntGeneration.AddListener(listener);
        }
    }
}