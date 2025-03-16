using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Assets.Ex3.Components;
using System.Collections.Generic;
using Unity.Burst;


// [BurstCompile]
public partial struct ChangePreyLifetimeSystem : ISystem
{

    EntityQuery preyQuery;
    EntityQuery predatorQuery;
    EntityQuery plantQuery;

    public void OnCreate(ref SystemState state) {
        preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        predatorQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PredatorTagComponent>());
        plantQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        state.RequireAnyForUpdate(preyQuery, predatorQuery, plantQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // EntityQuery preyQuery = state.GetEntityQuery(
        //     ComponentType.ReadWrite<LifetimeComponent>(),
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PreyTagComponent>());
        // EntityQuery predatorQuery = state.GetEntityQuery(
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PredatorTagComponent>());
        // EntityQuery plantQuery = state.GetEntityQuery(
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PlantTagComponent>());

        var preys = preyQuery.ToEntityArray(AllocatorManager.Temp);
        var predators = predatorQuery.ToEntityArray(AllocatorManager.Temp);
        var plants = plantQuery.ToEntityArray(AllocatorManager.Temp);

        for (int i = 0; i < preys.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(preys[i]);
            var preyPosition = SystemAPI.GetComponentRO<LocalTransform>(preys[i]);
            float decreasingFactor = 1.0f;
            for (int j = 0; j < plants.Length; j++)
            {
                var plantPosition = SystemAPI.GetComponentRO<LocalTransform>(plants[j]);
                if (Vector3.Distance(plantPosition.ValueRO.Position, preyPosition.ValueRO.Position) < Ex3Config.TouchingDistance)
                {
                    decreasingFactor *= 2;
                }
            }
            for (int j = 0; j < predators.Length; j++)
            {
                var predatorPosition = SystemAPI.GetComponentRO<LocalTransform>(predators[j]);
                if (Vector3.Distance(predatorPosition.ValueRO.Position, preyPosition.ValueRO.Position) < Ex3Config.TouchingDistance)
                {
                    decreasingFactor /= 2;
                }
            }
            lifetime.ValueRW.DecreasingFactor = decreasingFactor;
        }
        preys.Dispose();
        predators.Dispose();
        plants.Dispose();
    }
}
