#nullable enable
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
        private FadingEntity? FadingEntity { get; set; }

        public PheromoneInfo? Info { get; private set; }

        private void Start()
        {
            FadingEntity = new FadingEntity(transform.localScale, lifespan, minimum_size_percentage);
            FadingEntity.IsFaded.AddListener(DestroyGameObject);
        }

        public void Init(Identifier ant_identifier, uint intensity)
        {
            Info = new PheromoneInfo(ant_identifier, transform, new Intensity(intensity));
        }

        private void Update()
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            FadingEntity.Update(Time.deltaTime);
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
            transform.localScale = FadingEntity.LocalScale;
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}
