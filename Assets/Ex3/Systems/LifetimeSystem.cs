using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Assets.Ex3.Components;
using Unity.Burst;


// [BurstCompile]
public partial struct LifetimeSystem : ISystem
{
    EntityQuery query;

    public void OnCreate(ref SystemState state) {
        query = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<ReproduceComponent>());
        state.RequireForUpdate(query);
    }

    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entities = query.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < entities.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(entities[i]);
            lifetime.ValueRW.Lifetime -= SystemAPI.Time.DeltaTime * lifetime.ValueRO.DecreasingFactor;
            if (lifetime.ValueRO.Lifetime > 0) return;

            var reproduction = SystemAPI.GetComponentRW<ReproduceComponent>(entities[i]);
            if (reproduction.ValueRO.IsReproduced)
            {
                state.EntityManager.AddComponent<ToRespawnTagComponent>(entities[i]);
            }
            else
            {
                state.EntityManager.AddComponent<ToDeleteTagComponent>(entities[i]);
            }
        }
        entities.Dispose();
    }
}
