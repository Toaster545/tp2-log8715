using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        var toRegister = new List<ISystem>
        {
            new LifetimeSystem(),
            new PositionSystem(),
            new MovementSystem(),
            new ChangePlantLifetimeSystem(),
            new ChangePreyLifetimeSystem(),
            new ChangePredatorLifetimeSystem(),
            new PredatorMovementSystem(),
            new PreyMovementSystem(),
        };
        return toRegister;
    }
}
