using System;
using UnityEngine;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AntGenerationEnabler))]
    public class AntGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject AntTemplate;
        [SerializeField]
        private byte ant_count_at_start;
        [SerializeField]
        private byte ant_count_generated_when_enough_resources;
        [SerializeField]
        private byte ant_production_cost;

        void Start()
        {
            Generate(ant_count_at_start);
            var enabler = GetComponent<AntGenerationEnabler>();
            enabler.ListenToEnableAntGeneration(OnEnableAntGeneration, ant_production_cost);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Generate(byte count)
        {
            for (byte i = 0; i < count; ++i)
            {
                var ant = Instantiate(AntTemplate, transform.position, transform.rotation);
                var controller = ant.GetComponentInChildren<Controller>();
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