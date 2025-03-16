using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Assets.Ex3.Components;

// [BurstCompile]
public partial struct VelocitySystem : Unity.Entities.ISystem
{
    EntityQuery plantQuery;
    EntityQuery preyQuery;
    EntityQuery predatorQuery;

    public void OnCreate(ref SystemState state)
    {
        predatorQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PredatorTagComponent>());
        preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        plantQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
             ComponentType.ReadOnly<PlantTagComponent>());
        state.RequireAnyForUpdate(predatorQuery, preyQuery, plantQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        using (var plants = plantQuery.ToEntityArray(Allocator.TempJob))
        using (var predators = predatorQuery.ToEntityArray(Allocator.TempJob))
        using (var preys = preyQuery.ToEntityArray(Allocator.TempJob))
        {
            var plantPositions = new NativeArray<float3>(plants.Length, Allocator.TempJob);
            var predatorPositions = new NativeArray<float3>(predators.Length, Allocator.TempJob);
            var preyPositions = new NativeArray<float3>(preys.Length, Allocator.TempJob);

            var preyVelocities = new NativeArray<RefRW<VelocityComponent>>(preys.Length, Allocator.TempJob);
            var predatorVelocities = new NativeArray<RefRW<VelocityComponent>>(predators.Length, Allocator.TempJob);

            for (int i = 0; i < predators.Length; i++)
            {
                predatorPositions[i] = SystemAPI.GetComponentRW<LocalTransform>(predators[i]).ValueRO.Position;
                predatorVelocities[i] = SystemAPI.GetComponentRW<VelocityComponent>(predators[i]);
            }
            for (int i = 0; i < preys.Length; i++)
            {
                preyPositions[i] = SystemAPI.GetComponentRW<LocalTransform>(preys[i]).ValueRO.Position;
                preyVelocities[i] = SystemAPI.GetComponentRW<VelocityComponent>(preys[i]);
            }
            for (int i = 0; i < plants.Length; i++)
            {
                plantPositions[i] = SystemAPI.GetComponentRW<LocalTransform>(plants[i]).ValueRO.Position;
            }

            var moveJobPredator = new MoveJob
            {
                targetPositions = preyPositions,
                positions = predatorPositions,
                velocities = predatorVelocities,
                speed = Ex3Config.PredatorSpeed,
            };

            var moveJobPrey = new MoveJob
            {
                targetPositions = plantPositions,
                positions = preyPositions,
                velocities = preyVelocities,
                speed = Ex3Config.PreySpeed,
            };

            var jobHandlePredatorMovement = moveJobPredator.Schedule(predators.Length, 128);
            var jobHandlePreyMovement = moveJobPrey.Schedule(preys.Length, 128);

            JobHandle.CombineDependencies(jobHandlePredatorMovement, jobHandlePreyMovement).Complete();

            plantPositions.Dispose();
            preyPositions.Dispose();
            preyVelocities.Dispose();
            predatorPositions.Dispose();
            predatorVelocities.Dispose();
        }
    }
}