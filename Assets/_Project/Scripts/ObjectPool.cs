using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<Poolable<T>> activeObject = new List<Poolable<T>>();
    private Poolable<T> prefabObject;
    public Poolable<T> Create()
    {
        foreach (var obj in activeObject)
        {
            if (obj.IsAvailable())
            {
                obj.Reset();
                return obj;
            }
        }

        return CreateNewPoolObject();
    }
    public Poolable<T> CreateNewPoolObject()
    {
        Poolable<T> newObject = ObjectFactory.Instance.InstantiateTileDisplay().GetComponent<Poolable<T>>();
        activeObject.Add(newObject);
        return newObject;
    }
}