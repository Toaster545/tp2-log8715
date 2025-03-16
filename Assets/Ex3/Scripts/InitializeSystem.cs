using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Assets.Ex3.Components;

public partial struct InitializeSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SpawnerConfig>();
    }
    public void OnDestroy(ref SystemState state) {

    }
    public void OnUpdate(ref SystemState state) {
        SpawnerConfig config = SystemAPI.GetSingleton<SpawnerConfig>();
        var aliveEntities = SystemAPI.QueryBuilder().WithAll<LifetimeComponent>().Build(); 

        if (aliveEntities.IsEmpty){
            EntityManager entityManager = state.EntityManager;

            ComponentType[] plantComponentTypes = new ComponentType[]
            {
                ComponentType.ReadWrite<LifetimeComponent>(),
                ComponentType.ReadWrite<AlwaysReproduceTagComponent>(),
                ComponentType.ReadOnly<PlantTagComponent>(),
            };
            ComponentType[] preyComponentTypes = new ComponentType[]
            {
                ComponentType.ReadWrite<LifetimeComponent>(),
                ComponentType.ReadWrite<VelocityComponent>(),
                ComponentType.ReadOnly<PreyTagComponent>(),
            };
            ComponentType[] predatorComponentTypes = new ComponentType[]
            {
                ComponentType.ReadWrite<LifetimeComponent>(),
                ComponentType.ReadWrite<VelocityComponent>(),
                ComponentType.ReadOnly<PredatorTagComponent>(),
            };

            ComponentTypeSet plantComponentTypeSet = new ComponentTypeSet(plantComponentTypes);
            ComponentTypeSet preyComponentTypeSet = new ComponentTypeSet(preyComponentTypes);
            ComponentTypeSet predatorComponentTypeSet = new ComponentTypeSet(predatorComponentTypes);

            var plantPrefabEntity = config.plantPrefabEntity;
            var preyPrefabEntity = config.preyPrefabEntity;
            var predatorPrefabEntity = config.predatorPrefabEntity;

            var plantEntities = entityManager.Instantiate(plantPrefabEntity, config.plantCount, Allocator.Temp);
            var preyEntities = entityManager.Instantiate(preyPrefabEntity, config.preyCount, Allocator.Temp);
            var predatorEntities = entityManager.Instantiate(predatorPrefabEntity, config.predatorCount, Allocator.Temp);

            state.EntityManager.AddComponent(plantEntities, plantComponentTypeSet);
            state.EntityManager.AddComponent(preyEntities, preyComponentTypeSet);
            state.EntityManager.AddComponent(predatorEntities, predatorComponentTypeSet);
        }
    }
}