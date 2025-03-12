public class ChangePredatorLifetimeSystem : ISystem
{
    public void UpdateSystem()
    {
        EntityManager em = EntityManager.Instance;
        List<uint> predatorIds = em.GetEntitiesWithComponent(EntityManager.ComponentType.PreyTagComponent);
        List<uint> preyIds = em.GetEntitiesWithComponent(EntityManager.ComponentType.PredatorTagComponent);
        foreach (uint predator in predatorIds)
        {
            LifetimeComponent lifetime = em.GetComponent(predator, EntityManager.ComponentType.Lifetime) as LifetimeComponent;
            PositionComponent predatorPosition = em.GetComponent(predator, EntityManager.ComponentType.Position) as PositionComponent;
            if (lifetime != null)
            {
                lifetime.Lifetime = 1.0f;
            }

            foreach(uint prey in preyIds)
            {
                PositionComponent preyPosition = em.GetComponent(prey, EntityManager.ComponentType.Position) as PositionComponent;
                if (Vector3.Distance(predatorPosition.Position, preyPosition.Position) < Ex3Config.TouchingDistance)
                {
                    lifetime.DecreasingFactor /= 2;
                    break;
                }
            }
        }
    }
}
