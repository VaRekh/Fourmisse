using UnityEngine;

namespace Assets.Library
{
    public class Entity
    {
        private Transform Transform { get; }

        public Vector3 Position
            => Transform.position;

        public Entity(Transform transform)
        {
            Transform = transform;
        }
    }
}
