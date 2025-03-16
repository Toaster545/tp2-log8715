using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;


public partial struct PredatorMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        EntityQuery predatorQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PredatorTagComponent>());
        EntityQuery preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());
        state.RequireAnyForUpdate(predatorQuery, preyQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityQuery predatorQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PredatorTagComponent>());

        EntityQuery preyQuery = state.GetEntityQuery(
            ComponentType.ReadWrite<VelocityComponent>(),
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<PreyTagComponent>());

        var predators = predatorQuery.ToEntityArray(Allocator.Temp);
        var preys = preyQuery.ToEntityArray(Allocator.Temp);
        
        for (int i = 0; i < predators.Length; i++)
        {
            var predatorVelocity = SystemAPI.GetComponentRW<VelocityComponent>(predators[i]);
            var predatorPosition = SystemAPI.GetComponentRO<LocalTransform>(predators[i]);
            var closestDistance = float.MaxValue;
            var closestPosition = predatorPosition.ValueRO.Position;

            for (int j = 0; j < preys.Length; j++)
            {
                var preyPosition = SystemAPI.GetComponentRO<LocalTransform>(preys[j]);
                var distance = Vector3.Distance(preyPosition.ValueRO.Position, predatorPosition.ValueRO.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = preyPosition.ValueRO.Position;
                }
            }
            predatorVelocity.ValueRW.Velocity = (closestPosition - predatorPosition.ValueRO.Position) * Ex3Config.PredatorSpeed;
            predators.Dispose();
            preys.Dispose();
        }
    }
}
