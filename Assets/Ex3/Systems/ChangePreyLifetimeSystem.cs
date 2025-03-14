using UnityEngine;
using Assets.Ex3.Components;
using System.Collections.Generic;


public class ChangePreyLifetimeSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> preyIds = new List<uint>(em.PreyTagComponent.Keys);
        List<uint> predatorIds = new List<uint>(em.PredatorTagComponent.Keys);
        List<uint> plantIds = new List<uint>(em.PlantTagComponent.Keys);
        foreach (uint prey in preyIds)
        {
            LifetimeComponent lifetime = em.GetComponent(prey, EntityManager.ComponentType.Lifetime) as LifetimeComponent;
            PositionComponent preyPosition = em.GetComponent(prey, EntityManager.ComponentType.Position) as PositionComponent;
            if (lifetime != null)
            {
                lifetime.Lifetime = 1.0f;
            }

            foreach(uint plant in plantIds)
            {
                PositionComponent plantPosition = em.GetComponent(plant, EntityManager.ComponentType.Position) as PositionComponent;
                if (Vector3.Distance(plantPosition.Position, preyPosition.Position) < Ex3Config.TouchingDistance)
                {
                    lifetime.Lifetime /= 2;
                    break;
                }
            }

            foreach(uint predator in predatorIds)
            {
                PositionComponent predatorPosition = em.GetComponent(predator, EntityManager.ComponentType.Position) as PositionComponent;
                if (Vector3.Distance(predatorPosition.Position, preyPosition.Position) < Ex3Config.TouchingDistance)
                {
                    lifetime.Lifetime *= 2;
                    break;
                }
            }
        }
    }
}
