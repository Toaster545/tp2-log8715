using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Assets.Ex3.Components;

public partial struct DeleteEntitySystem : ISystem {

    EntityQuery toDeleteQuery; 
    public void OnCreate(ref SystemState state) {
        toDeleteQuery = state.GetEntityQuery(ComponentType.ReadOnly<ToDeleteTagComponent>());
        state.RequireForUpdate(toDeleteQuery);
    }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state) {
        var entities = toDeleteQuery.ToEntityArray(AllocatorManager.Temp);
        for(int i = 0; i < entities.Length; ++i)
        {
            state.EntityManager.DestroyEntity(entities[i]);
        }
        entities.Dispose();
    }
}
