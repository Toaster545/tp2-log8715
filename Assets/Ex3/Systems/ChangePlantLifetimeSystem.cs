using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Assets.Ex3.Components;
using System.Collections.Generic;
using Unity.Burst;


// [BurstCompile]
public partial struct ChangePlantLifetimeSystem : ISystem
{
    EntityQuery plantQuery;
    EntityQuery preyQuery;

    public void OnCreate(ref SystemState state) {
        plantQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        preyQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        state.RequireAnyForUpdate(plantQuery, preyQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // EntityQuery plantQuery = state.GetEntityQuery(
        //     ComponentType.ReadWrite<LifetimeComponent>(),
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PlantTagComponent>());
        // EntityQuery preyQuery = state.GetEntityQuery(
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PreyTagComponent>());

        var plants = plantQuery.ToEntityArray(Allocator.Temp);
        var preys = preyQuery.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < plants.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(plants[i]);
            var plantPosition = SystemAPI.GetComponentRO<LocalTransform>(plants[i]);
            float decreasingFactor = 1.0f;
            for (int j = 0; j < preys.Length; j++)
            {
                var preyPosition = SystemAPI.GetComponentRO<LocalTransform>(preys[j]);
                if (Vector3.Distance(plantPosition.ValueRO.Position, preyPosition.ValueRO.Position) < Ex3Config.TouchingDistance)
                {
                    decreasingFactor *= 2f;
                }
            }
            lifetime.ValueRW.DecreasingFactor = decreasingFactor;
        }

        plants.Dispose();
        preys.Dispose();
    }
}
