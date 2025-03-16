using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Assets.Ex3.Components;
using Unity.Collections;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public partial struct RespawnEntitySystem : ISystem {

    private const float StartingLifetimeLowerBound = 5;
    private const float StartingLifetimeUpperBound = 15;
    private const float StartingDecreasingFactor = 1;
    private const float StartingSize = 1;

    EntityQuery plantQuery; 
    EntityQuery preyQuery;
    EntityQuery predatorQuery; 
    public void OnCreate(ref SystemState state) {
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

    public void OnUpdate(ref SystemState state) {
        var plants = plantQuery.ToEntityArray(AllocatorManager.Temp);
        var preys = preyQuery.ToEntityArray(AllocatorManager.Temp);
        var predators = predatorQuery.ToEntityArray(AllocatorManager.Temp);

        SpawnerConfig config = SystemAPI.GetSingleton<SpawnerConfig>();

        int halfWidth = config.width / 2;
        int halfHeight = config.height / 2;

        foreach(var plant in plants) {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(plant);
            var lifetimeComponent = SystemAPI.GetComponentRW<LifetimeComponent>(plant);
            var reproductionComponent = SystemAPI.GetComponentRW<ReproduceComponent>(plant);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);
            transform.ValueRW.Scale = StartingSize;

            lifetimeComponent.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetimeComponent.ValueRW.Lifetime = lifetimeComponent.ValueRO.StartingLifetime;
            lifetimeComponent.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproductionComponent.ValueRW.IsReproduced = true;
            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(plant);
        }
        
        foreach(var prey in preys) {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(prey);
            var lifetimeComponent = SystemAPI.GetComponentRW<LifetimeComponent>(prey);
            var reproductionComponent = SystemAPI.GetComponentRW<ReproduceComponent>(prey);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);

            lifetimeComponent.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetimeComponent.ValueRW.Lifetime = lifetimeComponent.ValueRO.StartingLifetime;
            lifetimeComponent.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproductionComponent.ValueRW.IsReproduced = false;

            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(prey);
        }

        foreach(var predator in predators) {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(predator);
            var lifetimeComponent = SystemAPI.GetComponentRW<LifetimeComponent>(predator);
            var reproductionComponent = SystemAPI.GetComponentRW<ReproduceComponent>(predator);

            transform.ValueRW.Position = new float3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);

            lifetimeComponent.ValueRW.StartingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
            lifetimeComponent.ValueRW.Lifetime = lifetimeComponent.ValueRO.StartingLifetime;
            lifetimeComponent.ValueRW.DecreasingFactor = StartingDecreasingFactor;

            reproductionComponent.ValueRW.IsReproduced = false;

            state.EntityManager.RemoveComponent<ToRespawnTagComponent>(predator);
        }

        plants.Dispose();
        predators.Dispose();
        preys.Dispose();
    }
}
