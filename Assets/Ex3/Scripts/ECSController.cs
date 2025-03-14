using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ECSController : MonoBehaviour
{
    public List<ISystem> AllSystems => _allSystems;

    private readonly Dictionary<uint, GameObject> _gameObjectsForDisplay = new();
    private int _height;
    private int _width;

    public Ex3Config config;
    public GameObject predatorPrefab;
    public GameObject preyPrefab;
    public GameObject plantPrefab;

    #region Singleton
    private static ECSController _instance;
    public static ECSController Instance { get; private set; }
    // public static ECSController Instance
    // {
    //     get
    //     {
    //         if (_instance) return _instance;
    //         
    //         _instance = FindFirstObjectByType<ECSController>();
    //         if (!_instance)
    //         {
    //             Debug.LogError("Can't find ECSController instance in scene!!");
    //         }
    //         return _instance;
    //     }
    // }

    
    private ECSController() { }
    #endregion

    #region Public API
    public void UpdateSystemShapePosition(uint id, Vector2 position)
    {
        _gameObjectsForDisplay[id].transform.position = position;
    }


    public void UpdateSystemShapeSize(uint id, float size)
    {
        _gameObjectsForDisplay[id].transform.localScale = Vector2.one * size;
    }


    public void DestroyEntity(uint id)
    {
        _gameObjectsForDisplay[id].SetActive(false);
        EntityManager em = EntityManager.Instance;
        em.RemoveEntity(id);
    }


    public Vector2 GetRespawnPosition()
    {
        var halfWidth = _width / 2;
        var halfHeight = _height / 2;
        return new Vector2(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight));
    }

    // Fonction à modifier
    public void Respawn(Transform t)
    {
        // Vector2 position = GetRespawnPosition();
        t.position = GetRespawnPosition();
    }


    #endregion

    #region System Management
    private List<ISystem> _allSystems = new();
    private void Awake()
    {
        _allSystems = RegisterSystems.GetListOfSystems();
        Instance = this;
    }


    private void Start(){
        InitializeEntities();
    }


    private void InitializeEntities()
    {
        EntityManager em = EntityManager.Instance;
        var size = (float) config.gridSize;
        var ratio = Camera.main!.aspect;
        _height = (int)Mathf.Round(Mathf.Sqrt(size / ratio));
        _width = (int)Mathf.Round(size / _height);
        // Debug.LogWarning("Valeur de t.position:" + t.position);

        // Initialize Plants
        for (var i = 0; i < config.plantCount; i++)
        {
            var go = Create(plantPrefab);
            em.CreatePlantEntity(go);
            _gameObjectsForDisplay[(uint)i] = go;
        }

        // Initialize Prey
        for (var i = 0; i < config.preyCount; i++)
        {
            var go = Create(preyPrefab);
            em.CreatePreyEntity(go);
            _gameObjectsForDisplay[(uint)(i + config.plantCount)] = go;
        }

        // Initialize Predators
        for (var i = 0; i < config.predatorCount; i++)
        {
            var go = Create(predatorPrefab);
            em.CreatePredatorEntity(go);
            _gameObjectsForDisplay[(uint)(i + config.plantCount + config.preyCount)] = go;
        }
    }


    private void Update()
    {
        foreach (ISystem system in AllSystems)
        {
            system.UpdateSystem();
        }
    }


    private GameObject Create(GameObject prefab)
    {
        var go = Instantiate(prefab);
        Respawn(go.transform);
        return go;
    }
    #endregion
}



public interface ISystem
{
    void UpdateSystem();
}

public interface IComponent
{
}
