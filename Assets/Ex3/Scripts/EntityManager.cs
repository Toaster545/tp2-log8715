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


    public List<uint> EntityIds { get; private set; } = new List<uint>();

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

    private uint nextEntityId = 0;

    public uint CreateEntity(){
        uint id = nextEntityId++;
        EntityIds.Add(id);
        return id;
    }


    public void AddComponent(uint entityId, ComponentType type, IComponent component) {
        Dictionary<uint, IComponent> dict = GetDictionary(type);
        if (dict.ContainsKey(entityId)) {
            Debug.LogWarning($"Entity {entityId} already has a component of type {type}.");
        } else {
            dict.Add(entityId, component);
        }
    }


    public IComponent GetComponent(uint entityId, ComponentType type) {
        Dictionary<uint, IComponent> dict = GetDictionary(type);
        dict.TryGetValue(entityId, out IComponent comp);
        return comp;
    }


    public void SetComponent(uint entityId, ComponentType type, IComponent component) {
        Dictionary<uint, IComponent> dict = GetDictionary(type);
        dict[entityId] = component;
    }


    public bool RemoveComponent(uint entityId, ComponentType type) {
        Dictionary<uint, IComponent> dict = GetDictionary(type);
        return dict.Remove(entityId);
    }


    public void RemoveEntity(uint id)
    {
        EntityIds.Remove(id);
        RemoveComponent(id, ComponentType.Position);
        RemoveComponent(id, ComponentType.Velocity);
        RemoveComponent(id, ComponentType.Lifetime);
        RemoveComponent(id, ComponentType.Plant);
        RemoveComponent(id, ComponentType.Prey);
        RemoveComponent(id, ComponentType.Predator);
        RemoveComponent(id, ComponentType.Reproduced);
        RemoveComponent(id, ComponentType.AlwaysReproduce);
    }
}
