using UnityEngine;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Pheromone : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1000f)]
        private float lifespan;
        [SerializeField]
        private float minimum_size_percentage;

        private Stopwatch age;

        private CircleCollider2D circle_collider;
        float original_size;

        private float MinimumSize
            => minimum_size_percentage / 100f * original_size;

        private void Start()
        {
            age = new Stopwatch();
            circle_collider = GetComponent<CircleCollider2D>();
            original_size = circle_collider.radius;
        }

        private void Update()
        {
            age.Update(Time.deltaTime);

            float new_size = ComputeColliderSize
                (lifespan, age.CurrentValue, MinimumSize, original_size);
            circle_collider.radius = new_size;

            bool is_faded = age.CurrentValue >= lifespan;

            if (is_faded)
            {
                Destroy(gameObject);
            }

            static float ComputeColliderSize
                (float lifespan, float age, float minimum_size, float original_size)
            {
                float percentage_time_passed = Mathf.InverseLerp(0f, lifespan, age);
                float size = Mathf.Lerp(original_size, minimum_size, percentage_time_passed);

                return size;
            }
        }
    }
}
