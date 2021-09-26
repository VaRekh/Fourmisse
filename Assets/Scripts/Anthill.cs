using UnityEngine;


namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Anthill : MonoBehaviour
    {
        [SerializeField]
        private uint load;

        public void Store(uint load_given)
        {
            load += load_given;
        }
    }
}