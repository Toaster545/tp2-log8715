using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;


public partial struct PreyMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        EntityQuery plantQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        EntityQuery preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        state.RequireAnyForUpdate(plantQuery, preyQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityQuery plantQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PlantTagComponent>());
        EntityQuery preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());

        var preys = preyQuery.ToEntityArray(Allocator.Temp);
        var plants = plantQuery.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < preys.Length; i++)
        {
            var preyVelocity = SystemAPI.GetComponentRW<VelocityComponent>(preys[i]);
            var preyPosition = SystemAPI.GetComponentRO<LocalTransform>(preys[i]);
            var closestDistance = float.MaxValue;
            var closestPosition = preyPosition.ValueRO.Position;

            for (int j = 0; j < plants.Length; j++)
            {
                var plantPosition = SystemAPI.GetComponentRO<LocalTransform>(plants[j]);
                var distance = Vector3.Distance(plantPosition.ValueRO.Position, preyPosition.ValueRO.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = plantPosition.ValueRO.Position;
                }
            }
            preyVelocity.ValueRW.Velocity = (closestPosition - preyPosition.ValueRO.Position) * Ex3Config.PreySpeed;
            plants.Dispose();
            preys.Dispose();
        }
    }
}
