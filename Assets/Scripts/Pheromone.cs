using UnityEngine;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Pheromone : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("[O ; 1000] seconds")]
        [Range(0f, 1000f)]
        private float lifespan;
        [SerializeField]
        [Tooltip("[O - 100] %")]
        [Range(0f, 100f)]
        private float minimum_size_percentage;

        private Stopwatch age;

        float original_magnitude;
        Vector3 normalized_scale;

        private float MinimumMagnitude
            => minimum_size_percentage / 100f * original_magnitude;

        private void Start()
        {
            age = new Stopwatch();
            normalized_scale = transform.localScale.normalized;
            original_magnitude = transform.localScale.magnitude;
        }

        private void Update()
        {
            age.Update(Time.deltaTime);

            float new_magnitude = ComputeSize(lifespan, age.CurrentValue, MinimumMagnitude, original_magnitude);
            Vector3 new_scale = new_magnitude * normalized_scale;
            transform.localScale= new_scale;

            bool is_faded = age.CurrentValue >= lifespan;

            if (is_faded)
            {
                Destroy(gameObject);
            }

            static float ComputeSize
                (float lifespan, float age, float minimum_magnitude, float original_magnitude)
            {
                float percentage_time_passed = Mathf.InverseLerp(0f, lifespan, age);
                float magnitude = Mathf.Lerp(original_magnitude, minimum_magnitude, percentage_time_passed);

                return magnitude;
            }
        }
    }
}
