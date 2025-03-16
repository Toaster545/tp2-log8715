using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Assets.Ex3.Components;
using System.Collections.Generic;
using Unity.Burst;


// [BurstCompile]
public partial struct ChangePredatorLifetimeSystem : ISystem
{

    EntityQuery predatorQuery;
    EntityQuery preyQuery;
    public void OnCreate(ref SystemState state) {
        predatorQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PredatorTagComponent>());
        preyQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        state.RequireAnyForUpdate(predatorQuery, preyQuery);
    }
    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // EntityQuery predatorQuery = state.GetEntityQuery(
        //     ComponentType.ReadWrite<LifetimeComponent>(),
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PredatorTagComponent>());
        // EntityQuery preyQuery = state.GetEntityQuery(
        //     ComponentType.ReadOnly<LocalTransform>(),
        //     ComponentType.ReadOnly<PreyTagComponent>());

        var predators = predatorQuery.ToEntityArray(Allocator.Temp);
        var preys = preyQuery.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < predators.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(predators[i]);
            var predatorPosition = SystemAPI.GetComponentRO<LocalTransform>(predators[i]);
            float decreasingFactor = 1.0f;
            foreach (var prey in preys)
            {
                var preyPosition = SystemAPI.GetComponentRO<LocalTransform>(prey);
                if (Vector3.Distance(predatorPosition.ValueRO.Position, preyPosition.ValueRO.Position) < Ex3Config.TouchingDistance)
                {
                    decreasingFactor /= 2;
                }
            }
            lifetime.ValueRW.DecreasingFactor = decreasingFactor;
        }
        predators.Dispose();
        preys.Dispose();
    }
}
