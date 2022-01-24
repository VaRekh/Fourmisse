using UnityEngine;
using Assets.Library;


namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Resource : MonoBehaviour
    {
        [SerializeField]
        private UintReference load;

        public Collectable Collectable { get; private set; }

        private void Start()
        {
            Collectable = new Collectable(load);
        }
    }
}