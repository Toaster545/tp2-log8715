using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;


public class PredatorMovementSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> predatorIds = new List<uint>(em.PredatorTagComponent.Keys);
        foreach (uint predator in predatorIds)
        {
            PositionComponent position = em.GetComponent(predator, EntityManager.ComponentType.Position) as PositionComponent;
            var closestDistance = float.MaxValue;
            var closestPosition = position.Position;

            List<uint> preyIds = new List<uint>(em.PreyTagComponent.Keys);
            foreach (uint prey in preyIds)
            {
                PositionComponent preyPosition = em.GetComponent(prey, EntityManager.ComponentType.Position) as PositionComponent;
                var distance = Vector2.Distance(preyPosition.Position, position.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = preyPosition.Position;
                }
            }
            VelocityComponent velocity = em.GetComponent(predator, EntityManager.ComponentType.Velocity) as VelocityComponent;
            velocity.Velocity = (closestPosition - position.Position) * Ex3Config.PredatorSpeed;
            // velocity.Velocity = (closestPosition - position.Position) * velocity.Speed;
            em.SetComponent(predator, EntityManager.ComponentType.Velocity, velocity);
        }
    }
}
