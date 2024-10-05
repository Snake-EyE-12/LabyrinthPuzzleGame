using UnityEngine;

public abstract class Display<T> : MonoBehaviour
{
    public abstract void Render(T item);
}