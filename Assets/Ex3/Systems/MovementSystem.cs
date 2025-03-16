using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;


public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var query = state.GetEntityQuery(ComponentType.ReadOnly<VelocityComponent>(), ComponentType.ReadWrite<LocalTransform>());
        state.RequireForUpdate(query);
    }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var query = state.GetEntityQuery(ComponentType.ReadOnly<VelocityComponent>(), ComponentType.ReadWrite<LocalTransform>());
        if (query == null) return;

        float deltaTime = SystemAPI.Time.DeltaTime;
        var entities = query.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            var velocity = SystemAPI.GetComponentRO<VelocityComponent>(entities[i]);
            var position = SystemAPI.GetComponentRW<LocalTransform>(entities[i]);
            position.ValueRW.Position = position.ValueRW.Position + deltaTime * velocity.ValueRO.Velocity;
        }
        entities.Dispose();
    }
}
