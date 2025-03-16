using Unity.Entities;

namespace Assets.Ex3.Components
{
    public struct LifetimeComponent : IComponentData
    {
        public float Lifetime;
        public float StartingLifetime;
        public float DecreasingFactor;
    }
}
