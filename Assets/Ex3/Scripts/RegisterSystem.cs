using System.Collections.Generic;

public class RegisterSystem
{
    public static List<ISystem> GetListOfSystems()
    {
        var toRegister = new List<ISystem>
        {
            new LifetimeSystem(),
            // new ReproductionSystem(),
            new PositionSystem(),
        };
        return toRegister;
    }
}
