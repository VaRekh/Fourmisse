using UnityEngine;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Anthill : MonoBehaviour
    {
        [SerializeField]
        private uint load;

        public Storage Storage { get; private set; }

        private void Start()
        {
            Storage = new Storage(load);
        }
    }
}