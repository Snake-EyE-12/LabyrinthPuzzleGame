using System;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using UnityEngine;

public class EffectHolder : Singleton<EffectHolder>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private List<AnimEffect> effects = new();

    public void SpawnEffect(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        foreach (var effect in effects)
        {
            if (effect.name == name)
            {
                Instantiate(effect.prefab, position, rotation, parent);
            }
        }
    }

    public void SpawnEffect(string name, Transform t)
    {
        SpawnEffect(name, t.position, Quaternion.identity, t);
    }
    public void SpawnEffect(string name, Vector3 pos)
    {
        SpawnEffect(name, pos, Quaternion.identity, null);
    }
}

[System.Serializable]
public class AnimEffect
{
    public string name;
    public GameObject prefab;
}