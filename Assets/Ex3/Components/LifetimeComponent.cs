namespace Assets.Ex3.Components
{
    using UnityEngine;

    public class LifetimeComponent : IComponent
    {
        public float Lifetime { get; set; }
        public float StartingLifetime { get; set; }
        public float DecreasingFactor { get; set; }
    }
}
