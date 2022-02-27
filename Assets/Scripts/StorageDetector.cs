#nullable enable
using UnityEngine;
using UnityEngine.Events;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public class StorageDetector : MonoBehaviour
    {
        public UnityEvent<Storage> ContactWithStorageHappened { get; private set; } = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Anthill? anthill = collision.GetComponent<Anthill>();
            Storage? storage = anthill != null ? anthill.Storage : null;

            if (storage != null)
            {
                ContactWithStorageHappened.Invoke(storage);
            }
        }
    }
}