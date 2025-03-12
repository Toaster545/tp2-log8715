using System.Collections.Generic;
using UnityEngine;

public class ECSController : MonoBehaviour
{
    public List<ISystem> AllSystems => _allSystems;
    // Surement des trucs à rajouter par la

    #region Public API
    // Construire l'api publique ici
    // TODO: 
    //  Gérer le lifetime = 0 => mort
    //  Gérer la reproduction
    //  Modifier la position de la shape
    //  Modfier la taille de la shape
    #endregion

    #region System Management
    private List<ISystem> _allSystems = new();
    private void Awake()
    {
        _allSystems = RegisterSystems.GetListOfSystems();
    }

    private void Update()
    {
        foreach (ISystem system in AllSystems)
        {
            system.UpdateSystem();
        }
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
