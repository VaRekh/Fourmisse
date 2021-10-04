using UnityEngine;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Gland : MonoBehaviour
    {
        [SerializeField]
        private GameObject pheromone;
        [SerializeField]
        private float pheromone_per_second;

        private float GenerationInterval
            => 1f / pheromone_per_second;

        private Stopwatch stopwatch;

        private void Start()
        {
            stopwatch = new Stopwatch();
        }

        private void Update()
        {
            stopwatch.Update(Time.deltaTime);

            bool is_time_to_generate = stopwatch.CurrentValue >= GenerationInterval;

            if (is_time_to_generate)
            {
                Instantiate(pheromone, transform.position, pheromone.transform.rotation);
                stopwatch.Reset();
            }
        }
    }
}