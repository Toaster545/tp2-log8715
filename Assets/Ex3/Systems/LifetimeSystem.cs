using UnityEngine;
using Assets.Ex3.Components;


public class LifetimeSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;

        foreach (uint id in em.EntityIds)
        {
            LifetimeComponent comp = em.GetComponent(id, EntityManager.ComponentType.Lifetime) as LifetimeComponent;
            if (comp != null)
            {
                comp.Lifetime -= Time.deltaTime * comp.DecreasingFactor;
                if (comp.Lifetime >= 0) return;

                ReproducedTagComponent repTag = em.GetComponent(id, EntityManager.ComponentType.ReproducedTag) as ReproducedTagComponent;
                AlwaysReproduceTagComponent alwaysRepTag = em.GetComponent(id, EntityManager.ComponentType.AlwaysReproduceTag) as AlwaysReproduceTagComponent;
                if (alwaysRepTag != null || repTag != null)
                {
                    PositionComponent pos = em.GetComponent(id, EntityManager.ComponentType.Position) as PositionComponent;
                    pos.Position = ECSController.Instance.GetRespawnPosition();
                    em.SetComponent(id, EntityManager.ComponentType.Position, pos);
                }
                else
                {
                    ECSController.Instance.DestroyEntity(id);
                }
            }
        }
    }
}
