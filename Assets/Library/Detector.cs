using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Assets.Library
{
    public class Detector
    {
        private HashSet<Entity> DetectedEntities { get; set; }
        public UnityEvent<Entity, List<Entity>> EntityAppeared { get; private set; }
        public UnityEvent<Entity, List<Entity>> EntityVanished { get; private set; }

        public List<Entity> DetectedEntitiesAsList
            => new List<Entity>(DetectedEntities);

        public Detector()
        {
            EntityAppeared = new UnityEvent<Entity, List<Entity>>();
            DetectedEntities = new HashSet<Entity>();
            EntityVanished = new UnityEvent<Entity, List<Entity>>();
        }

        public void AddAppearingEntity(Entity entity)
        {
            Assert.IsNotNull(entity);

            bool is_element_added = DetectedEntities.Add(entity);
            if (is_element_added)
            {
                EntityAppeared.Invoke(entity, DetectedEntitiesAsList);
            }
        }

        public void RemoveVanishingEntity(Entity entity)
        {
            Assert.IsNotNull(entity);

            bool is_element_removed = DetectedEntities.Remove(entity);
            if (is_element_removed)
            {
                EntityVanished.Invoke(entity, DetectedEntitiesAsList);
            }
        }

        public Entity GetNearestEntity(List<Entity> entities, Vector2 ant_collider_position)
        {
            Assert.IsNotNull(entities);

            Entity nearest_entities = null;

            if (entities.Count > 0)
            {
                nearest_entities = entities[0];
                entities.RemoveAt(0);

                foreach (Entity entity in entities)
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

        public List<Entity> GetUntrackedEntities(List<Entity> entities, HashSet<Entity> tracked_entities)
        {
            List<Entity> untracked_entities = new List<Entity>();

            foreach (Entity candidate in entities)
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
