#nullable enable
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class PheromoneDetector : MonoBehaviour
    {
        public Detector<PheromoneInfo> Detector { get; private set; } = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);
            var entity = pheromone.Info;
            Assert.IsNotNull(entity);

#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            Detector.AddAppearingEntity(entity);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);
            var entity = pheromone.Info;
            Assert.IsNotNull(entity);

#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            Detector.RemoveVanishingEntity(entity);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
        }
    }
}
