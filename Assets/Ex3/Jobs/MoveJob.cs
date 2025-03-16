
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Assets.Ex3.Components;

// [BurstCompile]
public struct MoveJob : IJobParallelFor
{
    [ReadOnly] public float speed;
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> positions;
    public NativeArray<RefRW<VelocityComponent>> velocities;

    public void Execute(int index)
    {
        float closestDistance = float.MaxValue;
        float3 closestTarget = float3.zero;

        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (i == index) continue;

            float3 difference = (targetPositions[i] - positions[index]);
            float distanceSq = difference.x * difference.x + difference.y * difference.y;
            if (distanceSq < closestDistance)
            {
                closestDistance = distanceSq;
                closestTarget = targetPositions[i];
            }
        }

        float3 direction = (closestTarget - positions[index]);
        velocities[index].ValueRW.Velocity = math.normalizesafe(direction) * speed;
    }
}