namespace Assets.Library
{
    public class Storage
    {
        private uint Load { get; set; }

        public Storage(uint load)
        {
            Load = load;
        }
        
        public void Store(uint load_given)
        {
            Load += load_given;
        }
    }
}
