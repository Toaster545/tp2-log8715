using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Ex3.Components
{
    [BurstCompile]
    public struct VelocityComponent : IComponentData
    {
        public float3 Velocity;
    }
}
