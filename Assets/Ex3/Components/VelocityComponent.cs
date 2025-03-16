using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Ex3.Components
{
    public struct VelocityComponent : IComponentData
    {
        public float3 Velocity;
    }
}
