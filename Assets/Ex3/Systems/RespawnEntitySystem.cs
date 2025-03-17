using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Assets.Ex3.Components;
using Unity.Collections;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[BurstCompile]
public partial struct RespawnEntitySystem : ISystem
{

    private const float StartingLifetimeLowerBound = 5f;
    private const float StartingLifetimeUpperBound = 15f;
    private const float StartingDecreasingFactor = 1f;
    private const float StartingSize = 1f;

    EntityQuery plantQuery;
    EntityQuery preyQuery;
    EntityQuery predatorQuery;

    public void OnCreate(ref SystemState state)
    {
        plantQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadWrite<LocalTransform>(),
            ComponentType.ReadWrite<ReproduceComponent>(),
            ComponentType.ReadOnly<ToRespawnTagComponent>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadWrite<LocalTransform>(),
            ComponentType.ReadWrite<ReproduceComponent>(),
            ComponentType.ReadOnly<ToRespawnTagComponent>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        predatorQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadWrite<LocalTransform>(),
            ComponentType.ReadWrite<ReproduceComponent>(),
            ComponentType.ReadOnly<ToRespawnTagComponent>(),
            ComponentType.ReadOnly<PredatorTagComponent>());
        state.RequireAnyForUpdate(plantQuery, preyQuery, predatorQuery);
        state.RequireForUpdate<SpawnerConfig>();
    }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var plants = plantQuery.ToEntityArray(AllocatorManager.Temp);
        var preys = preyQuery.ToEntityArray(AllocatorManager.Temp);
        var predators = predatorQuery.ToEntityArray(AllocatorManager.Temp);

        SpawnerConfig config = SystemAPI.GetSingleton<SpawnerConfig>();

        int halfWidth = config.width / 2;
        int halfHeight = config.height / 2;

        foreach (var plant in plants)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(plant);
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(plant);
            var reproduction = SystemAPI.GetComponentRW<ReproduceComponent>(plant);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);
            transform.ValueRW.Scale = StartingSize;

            lifetime.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetime.ValueRW.Lifetime = lifetime.ValueRO.StartingLifetime;
            lifetime.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproduction.ValueRW.IsReproduced = true;
            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(plant);
        }

        foreach (var prey in preys)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(prey);
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(prey);
            var reproduction = SystemAPI.GetComponentRW<ReproduceComponent>(prey);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);

            lifetime.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetime.ValueRW.Lifetime = lifetime.ValueRO.StartingLifetime;
            lifetime.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproduction.ValueRW.IsReproduced = false;

            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(prey);
        }

        foreach (var predator in predators)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(predator);
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(predator);
            var reproduction = SystemAPI.GetComponentRW<ReproduceComponent>(predator);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);

            lifetime.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetime.ValueRW.Lifetime = lifetime.ValueRO.StartingLifetime;
            lifetime.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproduction.ValueRW.IsReproduced = false;

            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(predator);
        }

        plants.Dispose();
        predators.Dispose();
        preys.Dispose();
    }
}
