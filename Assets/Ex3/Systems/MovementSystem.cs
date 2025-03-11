using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;

public class MovementSystem : ISystem
{
    public void Update()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> movingEntities = new List<uint>(em.VelocityComponents.Keys);
        foreach (uint id in movingEntities)
        {
            VelocityComponent velocity = em.GetComponent(id, EntityManager.ComponentType.Velocity) as VelocityComponent;
            PositionComponent position = em.GetComponent(id, EntityManager.ComponentType.Position) as PositionComponent;
            position.Position += velocity.Velocity * Time.deltaTime;
        }
    }
}
