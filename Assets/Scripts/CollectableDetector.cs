#nullable enable
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class CollectableDetector : MonoBehaviour
    {
        public UnityEvent<Collectable> ContactWithCollectableLost { get; private set; } = new();
        public UnityEvent<Collectable> ContactWithNonEmptyCollectableHappened { get; private set; } = new();
        private HashSet<Collectable> DetectedNonEmptyCollectables { get; set; } = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Resource? resource = collision.GetComponent<Resource>();
            Collectable? collectable = resource.Collectable;
            if (collectable != null)
            {
                if (collectable.IsNotEmpty)
                {
                    DetectedNonEmptyCollectables.Add(collectable);
                    ContactWithNonEmptyCollectableHappened.Invoke(collectable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Resource? resource = collision.GetComponent<Resource>();
            Collectable? collectable = resource != null ? resource.Collectable : null;

            if (collectable != null)
            {
                var was_present = DetectedNonEmptyCollectables.Remove(collectable);
                if (was_present)
                {
                    ContactWithCollectableLost.Invoke(collectable);
                }
            }
        }
    }
}
