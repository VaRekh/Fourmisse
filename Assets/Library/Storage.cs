using UnityEngine.Events;

namespace Assets.Library
{
    public class Storage
    {
        private UintReference Load { get; set; }

        private UnityEvent<uint> LoadIncreased { get; set; }

        public Storage(UintReference load)
        {
            Load = load;
            LoadIncreased = new();
        }
        
        public void Store(uint load_given)
        {
            Load.Value += load_given;
            LoadIncreased.Invoke(Load.Value);
        }

        public void Consume(uint resource_quantity)
        {
            Load.Value -= resource_quantity;
        }

        public void ListenToLoadIncreased(UnityAction<uint> listener)
        {
            LoadIncreased.AddListener(listener);
        }
    }
}
