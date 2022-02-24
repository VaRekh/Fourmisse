using UnityEngine.Events;

namespace Assets.Library
{
    public class Collectable
    {
        private UintReference Load { get; set; }

        private UnityEvent completely_emtpied = new();

        public uint LoadLeft
        {
            get => Load.Value;
            private set => Load.Value = value;
        }

        public bool IsNotEmpty
            => LoadLeft != 0U;

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
            bool is_requirement_available = LoadLeft >= load_required;

            uint load_given = is_requirement_available ? load_required : LoadLeft;
            LoadLeft -= load_given;

            if (LoadLeft == 0U)
            {
                completely_emtpied.Invoke();
            }

            return load_given;
        }
    }
}