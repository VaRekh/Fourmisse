namespace Assets.Library
{
    public class Storage
    {
        private UintReference Load { get; set; }

        public Storage(UintReference load)
        {
            Load = load;
        }
        
        public void Store(uint load_given)
        {
            Load.Value += load_given;
        }
    }
}
