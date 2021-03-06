using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName= "Custom scriptable objects/Layer reference", fileName="LayerReference")]
    public class LayerReference : ScriptableObject
    {
        [SerializeField]
        private string layer;

        public string Layer
            => layer;

        public int Index
            => LayerMask.NameToLayer(layer);
    }
}
