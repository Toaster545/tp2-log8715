public class LifetimeSystem : ISystem
{
    public void Update()
    {
        EntityManager em = EntityManager.Instance;

        foreach (uint id in em.EntityIds)
        {
            LifetimeComponent comp = em.GetComponent(id, EntityManager.ComponentType.Lifetime) as LifetimeComponent;
            if (comp != null)
            {
                comp.Lifetime -= Time.deltaTime * comp.DecreasingFactor;
                if (comp.Lifetime >= 0) return;

                if (comp.Reproduced || comp.AlwaysReproduce)
                {
                    ECSController.Instance.ReproduceEntity(id);
                }
                else
                {
                    ECSController.Instance.DestroyEntity(id);
                }
            }
        }
    }
}
