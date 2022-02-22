﻿using UnityEngine;
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
        private FadingEntity FadingEntity { get; set; }

        public Entity Entity { get; private set; }
        public Identifier AntIdentifier { get; private set; }
        public float Intensity { get; private set; }

        private void Start()
        {
            Entity = new Entity(transform);
            FadingEntity = new FadingEntity(transform.localScale, lifespan, minimum_size_percentage);
            FadingEntity.IsFaded.AddListener(DestroyGameObject);
        }

        public void Init(Identifier ant_identifier, float intensity)
        {
            AntIdentifier = ant_identifier;
            Intensity = intensity;
        }

        private void Update()
        {
            FadingEntity.Update(Time.deltaTime);
            transform.localScale = FadingEntity.LocalScale;
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}
