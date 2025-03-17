using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine.UIElements;


[BurstCompile]
public partial struct MovementSystem : ISystem
{
    EntityQuery query;

    public void OnCreate(ref SystemState state)
    {
        query = state.GetEntityQuery(
            ComponentType.ReadOnly<VelocityComponent>(),
            ComponentType.ReadWrite<LocalTransform>());
        state.RequireForUpdate(query);
    }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (query == null) return;

        var entities = query.ToEntityArray(AllocatorManager.Temp);
        float deltaTime = SystemAPI.Time.DeltaTime;

        for (int i = 0; i < entities.Length; i++)
        {
            var velocity = SystemAPI.GetComponentRO<VelocityComponent>(entities[i]);
            var position = SystemAPI.GetComponentRW<LocalTransform>(entities[i]);
            position.ValueRW.Position = position.ValueRW.Position + deltaTime * velocity.ValueRO.Velocity;
        }
        entities.Dispose();
    }
}
