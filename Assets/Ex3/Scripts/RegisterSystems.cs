using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        var toRegister = new List<ISystem>
        {
            new PredatorMovementSystem(),
            new PreyMovementSystem(),
            new ChangePlantLifetimeSystem(),
            new ChangePreyLifetimeSystem(),
            new ChangePredatorLifetimeSystem(),
            new PositionSystem(),
            new LifetimeSystem(),
            new MovementSystem(),
        };
        return toRegister;
    }
}
