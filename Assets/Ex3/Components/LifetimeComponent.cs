using Unity.Burst;
using Unity.Entities;

namespace Assets.Ex3.Components
{
    [BurstCompile]
    public struct LifetimeComponent : IComponentData
    {
        public float Lifetime;
        public float StartingLifetime;
        public float DecreasingFactor;
    }
}
