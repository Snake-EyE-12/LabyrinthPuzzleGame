using Unity.VisualScripting;
using UnityEngine;

public abstract class Display<T> : MonoBehaviour
{
    protected T item;
    public virtual void Set(T item)
    {
        this.item = item;
        Render();
    }
    
    public abstract void Render();
}