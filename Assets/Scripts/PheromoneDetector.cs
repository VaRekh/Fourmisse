using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class PheromoneDetector : MonoBehaviour
    {
        private Detector<PheromoneInfo> detector;

        public Detector<PheromoneInfo> Detector
            => detector;

        private void Awake()
        {
            detector = new Detector<PheromoneInfo>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);
            var entity = pheromone.info;
            Assert.IsNotNull(entity);

            detector.AddAppearingEntity(entity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);
            var entity = pheromone.info;
            Assert.IsNotNull(entity);

            detector.RemoveVanishingEntity(entity);
        }
    }
}
