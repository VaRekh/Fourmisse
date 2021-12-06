namespace Assets.Library
{
    public class Collectable
    {
        private uint Load { get; set; }

        public Collectable(uint load)
        {
            Load = load;
        }

        public uint Collect(uint load_required)
        {
            bool is_requirement_available = Load >= load_required;

            uint load_given = is_requirement_available ? load_required : Load;
            Load -= load_given;

            return load_given;
        }
    }
}