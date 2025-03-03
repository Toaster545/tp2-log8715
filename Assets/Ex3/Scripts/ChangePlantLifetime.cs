﻿using UnityEngine;

public class ChangePlantLifetime : MonoBehaviour
{
    private Lifetime _lifetime;
    
    public void Start()
    {
        _lifetime = GetComponent<Lifetime>();
    }

    public void Update()
    {
        _lifetime.decreasingFactor = 1.0f;
        foreach(var prey in Ex4Spawner.PreyTransforms)
        {
            if (Vector3.Distance(prey.position, transform.position) < Ex3Config.TouchingDistance)
            {
                _lifetime.decreasingFactor *= 2f;
                break;
            }
        }
    }
}