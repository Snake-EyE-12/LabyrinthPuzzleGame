using System.Collections.Generic;
using UnityEngine;

public abstract class Collection<T> where T : class
{
    public abstract void Add(T item);
    public abstract void Insert(int index, T item);
    public abstract T Get();
    public abstract T GetAt(int index);
    public abstract void RemoveFirst();
    public abstract void RemoveLast();
    public abstract void RemoveAt(int index);
    public abstract void Shuffle();
    public abstract int Count();
    public abstract void Clear();
    public abstract void AddAll(Collection<T> collection);
    public abstract void Print();
}

#region Unqiue Collections
//
// public class QueuedCollection<T> : Collection<T> where T : class
// {
//     private Queue<T> queue = new Queue<T>();
//     public override void Add(T item)
//     {
//         queue.Enqueue(item);
//     }
//
//     public override void Insert(int index, T item)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public override T Get()
//     {
//         return queue.Peek();
//     }
//
//     public override void Remove()
//     {
//         queue.Dequeue();
//     }
// }
//
// public class StackCollection<T> : Collection<T> where T : class
// {
//     private Stack<T> stack = new Stack<T>();
//     public override void Add(T item)
//     {
//         stack.Push(item);
//     }
//
//     public override void Insert(int index, T item)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public override T Get()
//     {
//         return stack.Peek();
//     }
//
//     public override void Remove()
//     {
//         stack.Pop();
//     }
// }
#endregion
public class ListCollection<T> : Collection<T> where T : class
{
    private List<T> list = new List<T>();
    public override void Add(T item)
    {
        list.Add(item);
    }

    public override void Insert(int index, T item)
    {
        list.Insert(index, item);
    }

    public override T Get()
    {
        return list[0];
    }

    public override T GetAt(int index)
    {
        return list[index];
    }

    public override void RemoveLast()
    {
        list.RemoveAt(list.Count - 1);
    }
    public override void RemoveFirst()
    {
        list.RemoveAt(0);
    }
    public override void RemoveAt(int index)
    {
        list.RemoveAt(index);
    }

    public override void Shuffle()
    {
        for (int i = 0; i < list.Count - 1; i++) {
            var r = UnityEngine.Random.Range(i, list.Count - 1);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    public override int Count()
    {
        return list.Count;
    }

    public override void Clear()
    {
        list.Clear();
    }

    public override void AddAll(Collection<T> collection)
    {
        for (int i = 0; i < collection.Count(); i++)
        {
            Add(collection.GetAt(i));
        }
    }
    public override void Print()
    {
        string output = "";
        for (int i = 0; i < list.Count; i++)
        {
            output += list[i].ToString() + " ";
        }
        Debug.Log(output);
    }
}