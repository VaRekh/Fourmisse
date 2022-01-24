#nullable enable
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class CollectableDetector : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private readonly UnityEvent<Collectable>? contact_with_collectable_lost = new ();
        [SerializeField]
        [HideInInspector]
        private readonly UnityEvent<Collectable>? contact_with_non_empty_collectable_happened = new();

        [SerializeField]
        [HideInInspector]
        private readonly HashSet<Collectable> detected_non_empty_collectables = new();

        public UnityEvent<Collectable>? ContactWithCollectableLost
            => contact_with_collectable_lost;
        public UnityEvent<Collectable>? ContactWithNonEmptyCollectableHappened
            => contact_with_non_empty_collectable_happened;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Collectable? collectable = collision.GetComponent<Resource>()?.Collectable;
            if (collectable != null)
            {
                if (collectable.IsNotEmpty)
                {
                    detected_non_empty_collectables.Add(collectable);
                    Assert.IsNotNull(contact_with_non_empty_collectable_happened);
                    contact_with_non_empty_collectable_happened?.Invoke(collectable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Collectable? collectable = collision.GetComponent<Resource>()?.Collectable;
            if (collectable != null)
            {
                var was_present = detected_non_empty_collectables.Remove(collectable);
                if (was_present)
                {
                    Assert.IsNotNull(contact_with_collectable_lost);
                    contact_with_collectable_lost?.Invoke(collectable);
                }
            }
        }
    }
}
