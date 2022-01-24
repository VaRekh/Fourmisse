#nullable enable
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class StorageDetector : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private readonly UnityEvent<Storage>? contact_with_storage_happened = new();

        public UnityEvent<Storage>? ContactWithStorageHappened
            => contact_with_storage_happened;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Storage? storage = collision.GetComponent<Anthill>()?.Storage;
            if (storage != null)
            {
                Assert.IsNotNull(contact_with_storage_happened);
                contact_with_storage_happened?.Invoke(storage);
            }
        }
    }
}