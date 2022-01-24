using UnityEngine.Events;

namespace Assets.Library
{
    public class Collectable
    {
        private UintReference Load { get; set; }

        private UnityEvent completely_emtpied = new();

        public bool IsNotEmpty
            => Load.Value != 0U;

        public Collectable(UintReference load)
        {
            Load = load;
        }

        public void ListenToCollectableCompletelyEmptied(UnityAction listener)
        {
            completely_emtpied.AddListener(listener);
        }

        public void StopListeningToCollectableCompletelyEmptied(UnityAction listener)
        {
            completely_emtpied.RemoveListener(listener);
        }

        public uint Collect(uint load_required)
        {
            bool is_requirement_available = Load.Value >= load_required;

            uint load_given = is_requirement_available ? load_required : Load.Value;
            Load.Value -= load_given;

            if (Load.Value == 0U)
            {
                completely_emtpied.Invoke();
            }

            return load_given;
        }
    }
}