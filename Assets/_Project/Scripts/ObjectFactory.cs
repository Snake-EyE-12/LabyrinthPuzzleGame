using System.Collections;
using Guymon.DesignPatterns;
using UnityEngine;

public class ObjectFactory : Singleton<ObjectFactory>
{
    [SerializeField] private TileDisplay tileDisplay;
    private ObjectPool<TileDisplay> tilePool = new ObjectPool<TileDisplay>();
    public TileDisplay GetTileDisplay()
    {
        return tilePool.Create().GetSelf();
    }
    public TileDisplay InstantiateTileDisplay()
    {
        return Instantiate(tileDisplay, Vector3.zero, Quaternion.identity);
    }
}

public interface Poolable<T> where T : Component
{
    public T GetSelf();
    public void Reset();
    public bool IsAvailable();
}