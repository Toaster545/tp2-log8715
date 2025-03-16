using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Assets.Ex3.Components;
using Unity.Transforms;
using UnityEngine.Rendering;

public partial struct PlantSizeSystem : ISystem
{
    EntityQuery plantQuery;

    public void OnCreate(ref SystemState state) {
        plantQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LifetimeComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        state.RequireForUpdate(plantQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entities = plantQuery.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < entities.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRO<LifetimeComponent>(entities[i]);
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entities[i]);
            transform.ValueRW.Scale = lifetime.ValueRO.Lifetime / lifetime.ValueRO.StartingLifetime;
        }
        entities.Dispose();
    }
}