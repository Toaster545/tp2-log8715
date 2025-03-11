namespace Assets.Ex3.Components
{
    using UnityEngine;

    public class LifetimeComponent : IComponent
    {
        public float Lifetime { get; set; }
        public float StartingLifetime { get; }
        public bool AlwaysReproduce { get; set; }
        public bool Reproduced { get; set; }
        public float DecreasingFactor { get; set; }
    }
}
