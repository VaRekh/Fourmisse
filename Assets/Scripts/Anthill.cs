#nullable enable
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Library;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Anthill : MonoBehaviour
    {
        [SerializeField]
        private UintReference load = new();

        public Storage? Storage { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(load);
            Storage = new Storage(load);
        }
    }
}