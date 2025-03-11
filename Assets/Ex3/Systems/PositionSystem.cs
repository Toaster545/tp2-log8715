public class PositionSystem : ISystem
{
    public void Update()
    {
        EntityManager em = EntityManager.Instance;

        foreach (uint id in em.EntityIds)
        {
            PositionComponent comp = em.GetComponent(id, EntityManager.ComponentType.Position) as PositionComponent;
            if (comp != null)
            {
                ECSController.Instance.UpdateShapePosition(id, comp.Position);
            }
        }
    }
}
