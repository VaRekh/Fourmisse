#nullable enable
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Anthill))]
    public class AntGenerationEnabler : MonoBehaviour
    {
        private uint load_required_for_generation;
        private UnityEvent<Action<uint>> EnableAntGeneration { get; set; } = new();
        private Storage? Storage { get; set; }

        public void Start()
        {
            var storage = GetComponent<Anthill>().Storage;
            Assert.IsNotNull(storage);
            Storage = storage;
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            Storage.ListenToLoadIncreased(CheckLoad);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        private void CheckLoad(uint load)
        {
            bool anthill_has_enough_resources = load >= load_required_for_generation;
            if (anthill_has_enough_resources)
            {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
                EnableAntGeneration.Invoke(Storage.Consume);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
            }
        }

        public void ListenToEnableAntGeneration(UnityAction<Action<uint>> listener, uint load_required)
        {
            load_required_for_generation = load_required;
            EnableAntGeneration.AddListener(listener);
        }
    }
}