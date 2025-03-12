using System;
using System.Collections.Generic;
using Assets.Ex3.Components;

public class EntityManager {
    #region Singleton
    private static EntityManager _instance;
    public static EntityManager Instance {
        get {
            if (_instance == null) {
                _instance = new EntityManager();
            }
            return _instance;
        }
    }
    private EntityManager() { }
    #endregion

    public List<uint> EntitiesIds { get; private set; } = new List<uint>();

    public Dictionary<uint, IComponent> LifetimeComponents { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> PositionComponents { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> VelocityComponents { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> PlantTagComponent { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> PreyTagComponent { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> PredatorTagComponent { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> ReproducedTagComponent { get; private set; } = new Dictionary<uint, IComponent>();
    public Dictionary<uint, IComponent> AlwaysReproduceTagComponent { get; private set; } = new Dictionary<uint, IComponent>();


    public enum ComponentType {
        Lifetime,
        Position,
        Velocity,
        Plant,
        Prey,
        Predator,
        Reproduced,
        AlwaysReproduce,
    }


    private Dictionary<uint, IComponent> GetDictionary(ComponentType type) {
        switch (type) {
            case ComponentType.Position:
                return PositionComponents;
            case ComponentType.Velocity:
                return VelocityComponents;
            case ComponentType.Lifetime:
                return LifetimeComponents;
            case ComponentType.Plant:
                return PlantTagComponent;
            case ComponentType.Prey:
                return PreyTagComponent;
            case ComponentType.Predator:
                return PredatorTagComponent;
            case ComponentType.Reproduced:
                return ReproducedTagComponent;
            case ComponentType.AlwaysReproduce:
                return AlwaysReproduceTagComponent;
            default:
                throw new ArgumentException($"Invalid component type: {type}");
        }
    }

    public IComponent GetComponent(uint entityId, ComponentType type) {
        Dictionary<uint, IComponent> dict = GetDictionary(type);
        dict.TryGetValue(entityId, out IComponent comp);
        return comp;
    }
}
