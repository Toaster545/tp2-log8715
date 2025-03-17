using Unity.Burst;
using Unity.Entities;

namespace Assets.Ex3.Components
{
    [BurstCompile]
    public struct ReproduceComponent : IComponentData
    {
        public bool IsReproduced;
    }
}
