using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;


public class PreyMovementSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> preyIds = new List<uint>(em.PreyTagComponent.Keys);
        foreach (uint prey in preyIds)
        {
            PositionComponent position = em.GetComponent(prey, EntityManager.ComponentType.Position) as PositionComponent;

            var closestDistance = float.MaxValue;
            var closestPosition = position.Position;

            List<uint> plantIds = new List<uint>(em.PlantTagComponent.Keys);
            foreach (uint plant in plantIds)
            {
                PositionComponent plantPosition = em.GetComponent(plant, EntityManager.ComponentType.Position) as PositionComponent;
                var distance = Vector2.Distance(plantPosition.Position, position.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = plantPosition.Position;
                }
            }
            VelocityComponent velocity = em.GetComponent(prey, EntityManager.ComponentType.Velocity) as VelocityComponent;
            velocity.Velocity = (closestPosition - position.Position) * Ex3Config.PreySpeed;
            em.SetComponent(prey, EntityManager.ComponentType.Velocity, velocity);
        }
    }
}

