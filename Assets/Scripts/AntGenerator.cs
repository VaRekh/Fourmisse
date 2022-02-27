#nullable enable
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AntGenerationEnabler))]
    public class AntGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject? ant_template;
        [SerializeField]
        private byte ant_count_at_start;
        [SerializeField]
        private byte ant_count_generated_when_enough_resources;
        [SerializeField]
        private byte ant_production_cost;

        void Start()
        {
            Assert.IsNotNull(ant_template);
            Generate(ant_count_at_start);
            var enabler = GetComponent<AntGenerationEnabler>();
            enabler.ListenToEnableAntGeneration(OnEnableAntGeneration, ant_production_cost);
        }

        private void Generate(byte count)
        {
            for (byte i = 0; i < count; ++i)
            {
                var ant = Instantiate(ant_template, transform.position, transform.rotation);
                Assert.IsNotNull(ant);
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
                var controller = ant.GetComponentInChildren<Controller>();
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
                Assert.IsNotNull(controller);
                controller.Anthill = transform;
            }
        }

        private void OnEnableAntGeneration(Action<uint> consume_resources)
        {
            consume_resources(ant_production_cost);
            Generate(ant_count_generated_when_enough_resources);

        }
    }
}