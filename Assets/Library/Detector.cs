#nullable enable
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Assets.Library
{
    public class Detector<T>
        where T : class, ITrackable
    {
        private HashSet<T> DetectedEntities { get; set; }
        public UnityEvent<T, List<T>> EntityAppeared { get; private set; }
        public UnityEvent<T, List<T>> EntityVanished { get; private set; }

        public List<T> DetectedEntitiesAsList
            => new (DetectedEntities);

        public Detector()
        {
            EntityAppeared = new UnityEvent<T, List<T>>();
            DetectedEntities = new HashSet<T>();
            EntityVanished = new UnityEvent<T, List<T>>();
        }

        public void AddAppearingEntity(T entity)
        {
            Assert.IsNotNull(entity);

            bool is_element_added = DetectedEntities.Add(entity);
            if (is_element_added)
            {
                EntityAppeared.Invoke(entity, DetectedEntitiesAsList);
            }
        }

        public void RemoveVanishingEntity(T entity)
        {
            Assert.IsNotNull(entity);

            bool is_element_removed = DetectedEntities.Remove(entity);
            if (is_element_removed)
            {
                EntityVanished.Invoke(entity, DetectedEntitiesAsList);
            }
        }

        public T? GetNearestEntity(List<T> entities, Vector2 ant_collider_position)
        {
            Assert.IsNotNull(entities);

            T? nearest_entities = null;

            if (entities.Count > 0)
            {
                nearest_entities = entities[0];
                entities.RemoveAt(0);

                foreach (T entity in entities)
                {
                    Vector2 nearest_entity_position = nearest_entities.Position;
                    Vector2 from_ant_to_nearest_entity = ant_collider_position.To(nearest_entity_position);


                    Vector2 entity_position = entity.Position;
                    Vector2 from_ant_to_entity = ant_collider_position.To(entity_position);

                    LengthComparison result = from_ant_to_entity.CompareTo(from_ant_to_nearest_entity);
                    if (result == LengthComparison.ShorterThan)
                    {
                        nearest_entities = entity;
                    }
                }
            }

            return nearest_entities;
        }

        public List<T> GetUntrackedEntities(List<T> entities, HashSet<T> tracked_entities)
        {
            var untracked_entities = new List<T>();

            foreach (T candidate in entities)
            {
                bool is_entity_tracked = tracked_entities.Contains(candidate);
                if (!is_entity_tracked)
                {
                    untracked_entities.Add(candidate);
                }
            }

            return untracked_entities;
        }
    }
}
