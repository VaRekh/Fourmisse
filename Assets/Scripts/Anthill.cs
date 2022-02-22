using UnityEngine;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Anthill : MonoBehaviour
    {
        [SerializeField]
        private UintReference load;

        public Storage Storage { get; private set; }

        private void Awake()
        {
            Storage = new Storage(load);
        }
    }
}