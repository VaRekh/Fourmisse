using UnityEngine;


namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField]
        private uint load;

        public uint Collect(uint load_required)
        {
            bool is_requirement_available = load >= load_required;

            uint load_given = is_requirement_available ? load_required : load;
            load -= load_given;

            return load_given;
        }
    }
}