using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class PheromoneDetector : MonoBehaviour
    {
        private HashSet<Pheromone> DetectedPheromones { get; set; }
        public UnityEvent<Pheromone, List<Pheromone>> PheromoneAppeared { get; private set; }
        public UnityEvent<Pheromone, List<Pheromone>> PheromoneVanished { get; private set; }

        public List<Pheromone> DetectedPheromonesAsList
            => new List<Pheromone>(DetectedPheromones);

        private void Start()
        {
            PheromoneAppeared = new UnityEvent<Pheromone, List<Pheromone>>();
            DetectedPheromones = new HashSet<Pheromone>();
            PheromoneVanished = new UnityEvent<Pheromone, List<Pheromone>>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);

            bool is_element_added = DetectedPheromones.Add(pheromone);
            if (is_element_added)
            {
                PheromoneAppeared.Invoke(pheromone, DetectedPheromonesAsList);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var pheromone = collision.gameObject.GetComponent<Pheromone>();
            Assert.IsNotNull(pheromone);

            bool is_element_removed = DetectedPheromones.Remove(pheromone);
            if (is_element_removed)
            {
                 PheromoneVanished.Invoke(pheromone, DetectedPheromonesAsList);
            }
        }

        public Pheromone GetNearestPheromone(List<Pheromone> pheromones, Vector2 ant_collider_position)
        {
            Assert.IsNotNull(pheromones);

            Pheromone nearest_pheromone = null;

            if (pheromones.Count > 0)
            {
                nearest_pheromone = pheromones[0];
                pheromones.RemoveAt(0);

                foreach (Pheromone pheromone in pheromones)
                {
                    Vector2 nearest_pheromone_position = nearest_pheromone.transform.position;
                    Vector2 from_ant_to_nearest_pheromone = ant_collider_position.To(nearest_pheromone_position);


                    Vector2 pheromone_position = pheromone.transform.position;
                    Vector2 from_ant_to_pheromone = ant_collider_position.To(pheromone_position);

                    LengthComparison result = from_ant_to_pheromone.CompareTo(from_ant_to_nearest_pheromone);
                    if (result == LengthComparison.ShorterThan)
                    {
                        nearest_pheromone = pheromone;
                    }
                }
            }

            return nearest_pheromone;
        }

        public List<Pheromone> GetUntrackedPheromones(List<Pheromone> pheromones, HashSet<Pheromone> tracked_pheromones)
        {
            List<Pheromone> untracked_pheromones = new List<Pheromone>();

            foreach (Pheromone candidate in pheromones)
            {
                bool is_pheromone_tracked = tracked_pheromones.Contains(candidate);
                if (!is_pheromone_tracked)
                {
                    untracked_pheromones.Add(candidate);
                }
            }

            return untracked_pheromones;
        }
    }
}
