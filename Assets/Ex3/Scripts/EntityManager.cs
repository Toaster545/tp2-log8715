using System;
using System.Collections.Generic;
using Assets.Ex3.Components;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityManager {

    private const float StartingLifetimeLowerBound = 5;
    private const float StartingLifetimeUpperBound = 15;

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
        ReproducedTag,
        AlwaysReproduceTag,
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
            case ComponentType.ReproducedTag:
                return ReproducedTagComponent;
            case ComponentType.AlwaysReproduceTag:
                return AlwaysReproduceTagComponent;
            default:
                throw new ArgumentException($"Invalid component type: {type}");
        }
    }

    private uint nextEntityId = 0;

    public void CreatePlantEntity(GameObject prefab) {
        uint id = CreateEntity();
        float startingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
        LifetimeComponent lifetime = new LifetimeComponent();
        lifetime.Lifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
        lifetime.DecreasingFactor = 1;
        lifetime.StartingLifetime = lifetime.Lifetime;
        AddComponent(id, ComponentType.Lifetime, lifetime);

        PositionComponent position = new PositionComponent();
        position.Position = new Vector2(
                prefab.transform.position.x,
                prefab.transform.position.y);
        AddComponent(id, ComponentType.Position, position);

        AddComponent(id, ComponentType.Plant, new PlantTagComponent());
        AddComponent(id, ComponentType.AlwaysReproduceTag, new AlwaysReproduceTagComponent());
    }


    // Fonction à finir
    public void CreatePreyEntity(GameObject prefab) {
        uint id = CreateEntity();
        LifetimeComponent lifetime = new LifetimeComponent();
        lifetime.Lifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
        lifetime.DecreasingFactor = 1;
        lifetime.StartingLifetime = lifetime.Lifetime;
        AddComponent(id, ComponentType.Lifetime, lifetime);

        PositionComponent position = new PositionComponent();
        position.Position = new Vector2(
                prefab.transform.position.x,
                prefab.transform.position.y);
        AddComponent(id, ComponentType.Position, position);

        VelocityComponent velocity = new VelocityComponent();

        AddComponent(id, ComponentType.Prey, new PreyTagComponent());
    }


    // Fonction à finir
    public void CreatePredatorEntity(GameObject prefab) {
        uint id = CreateEntity();
        LifetimeComponent lifetime = new LifetimeComponent();
        lifetime.Lifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
        lifetime.DecreasingFactor = 1;
        lifetime.StartingLifetime = lifetime.Lifetime;
        AddComponent(id, ComponentType.Lifetime, lifetime);

        PositionComponent position = new PositionComponent();
        position.Position = new Vector2(
                prefab.transform.position.x,
                prefab.transform.position.y);
        AddComponent(id, ComponentType.Position, position);

        VelocityComponent velocity = new VelocityComponent();

        AddComponent(id, ComponentType.Predator, new PredatorTagComponent());
    }


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
        RemoveComponent(id, ComponentType.ReproducedTag);
        RemoveComponent(id, ComponentType.AlwaysReproduceTag);
    }
}
