using UnityEngine;

namespace Assets.Library
{
    public class PheromoneInfo : ITrackable
    {
        private Intensity PheromoneIntensity { get; set; }
        private Transform Transform { get; set; }
        public Identifier AntIdentifier { get; private set; }
        public Vector3 Position
            => Transform.position;

        public uint Intensity
            => PheromoneIntensity.Value;

        public PheromoneInfo(Identifier ant_identifier, Transform transform, Intensity intensity)
        {
            AntIdentifier = ant_identifier;
            Transform = transform;
            PheromoneIntensity = intensity;
        }
    }
}
