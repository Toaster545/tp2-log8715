using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using System;

public class EntitySpawner : MonoBehaviour {
    public Ex3Config config;
    public GameObject predatorPrefab;
    public GameObject preyPrefab;
    public GameObject plantPrefab;

    class Baker : Baker<EntitySpawner> {
        public override void Bake(EntitySpawner spawner){
            var size = (float)spawner.config.gridSize;
            // var ratio = Camera.main!.aspect;
            float ratio = 16f/9f;
            int _height = (int)Math.Round(Math.Sqrt(size / ratio));
            int _width = (int)Math.Round(size / _height);
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerConfig
            {
                plantPrefabEntity = GetEntity(spawner.plantPrefab, TransformUsageFlags.Dynamic),
                predatorPrefabEntity = GetEntity(spawner.predatorPrefab, TransformUsageFlags.Dynamic),
                preyPrefabEntity = GetEntity(spawner.preyPrefab, TransformUsageFlags.Dynamic),
                plantCount = spawner.config.plantCount,
                predatorCount = spawner.config.predatorCount,
                preyCount = spawner.config.preyCount,
                height = _height,
                width = _width
            });
        }
    }
}

struct SpawnerConfig : IComponentData
{
    public Entity predatorPrefabEntity;
    public Entity preyPrefabEntity;
    public Entity plantPrefabEntity;
    public int height;
    public int width;
    public int plantCount;
    public int predatorCount;
    public int preyCount;
}