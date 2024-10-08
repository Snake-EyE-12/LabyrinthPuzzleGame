using Unity.VisualScripting;
using UnityEngine;

public abstract class Display<T> : MonoBehaviour
{
    protected T item;
    public void Set(T item)
    {
        this.item = item;
        Render(item);
    }
    
    public abstract void Render(T item);
}