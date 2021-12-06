using UnityEngine;
using UnityEngine.Events;

namespace Assets.Library
{
    public class FadingEntity
    {
        private float Lifespan { get; }
        private float MinimumSizePercentage { get; }
        private float OriginalMagnitude { get; }
        private Vector3 NormalizedScale { get; }
        private Stopwatch Age { get; set; }
        private float MinimumMagnitude
            => MinimumSizePercentage / 100f * OriginalMagnitude;

        public Vector3 LocalScale { get; set; }

        public UnityEvent IsFaded { get; private set; }


        public FadingEntity(Vector3 local_scale, float lifespan, float minimum_size_percentage)
        {
            Lifespan = lifespan;
            MinimumSizePercentage = minimum_size_percentage;
            NormalizedScale = local_scale.normalized;
            OriginalMagnitude = local_scale.magnitude;

            Age = new Stopwatch();
            IsFaded = new UnityEvent();
        }

        public void Update(float delta_time)
        {
            Age.Update(delta_time);

            float new_magnitude = ComputeSize(Lifespan, Age.CurrentValue, MinimumMagnitude, OriginalMagnitude);
            Vector3 new_scale = new_magnitude * NormalizedScale;
            LocalScale = new_scale;

            bool is_faded = Age.CurrentValue >= Lifespan;

            if (is_faded)
            {
                IsFaded.Invoke();
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
