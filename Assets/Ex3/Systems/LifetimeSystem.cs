using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Assets.Ex3.Components;


public partial struct LifetimeSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        EntityQuery query = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<ReproducedTagComponent>(),
            ComponentType.ReadOnly<AlwaysReproduceTagComponent>());
        state.RequireForUpdate(query);
    }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityQuery query = state.GetEntityQuery(
            ComponentType.ReadWrite<LifetimeComponent>(),
            ComponentType.ReadOnly<ReproducedTagComponent>(),
            ComponentType.ReadOnly<AlwaysReproduceTagComponent>());

        var entities = query.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < entities.Length; i++)
        {
            var lifetime = SystemAPI.GetComponentRW<LifetimeComponent>(entities[i]);
            lifetime.ValueRW.Lifetime -= SystemAPI.Time.DeltaTime * lifetime.ValueRO.DecreasingFactor;
            if (lifetime.ValueRO.Lifetime >= 0) return;

            state.EntityManager.DestroyEntity(entities[i]);
            /*
            var repTag = SystemAPI.GetComponentRO<ReproducedTagComponent>(entities[i]);
            var alwaysRepTag = SystemAPI.GetComponentRO<AlwaysReproduceTagComponent>(entities[i]);

            // Peut être modifier par la
            if (alwaysRepTag.IsEmpty || repTag != null)
            {
                SystemAPI.RespawnEntity(entities[i]);
            }
            else
            {
                SystemAPI.RemoveEntity(entities[i]);
            }
            */
        }
        entities.Dispose();
    }
}
