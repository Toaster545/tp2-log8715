public class ChangePlantLifetimeSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> plantIds = em.GetEntitiesWithComponent(EntityManager.ComponentType.PreyTagComponent);
        List<uint> preyIds = em.GetEntitiesWithComponent(EntityManager.ComponentType.PredatorTagComponent);
        foreach (uint plant in plantIds)
        {
            LifetimeComponent lifetime = em.GetComponent(plant, EntityManager.ComponentType.Lifetime) as LifetimeComponent;
            PositionComponent plantPosition = em.GetComponent(plant, EntityManager.ComponentType.Position) as PositionComponent;
            if (lifetime != null)
            {
                lifetime.Lifetime = 1.0f;
            }

            foreach(uint prey in preyIds)
            {
                PositionComponent preyPosition = em.GetComponent(prey, EntityManager.ComponentType.Position) as PositionComponent;
                if (Vector3.Distance(plantPosition.Position, preyPosition.Position) < Ex3Config.TouchingDistance)
                {
                    lifetime.Lifetime *= 2;
                    break;
                }
            }
        }
    }
}
