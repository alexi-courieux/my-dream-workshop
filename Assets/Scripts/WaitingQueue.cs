using UnityEngine;
using Utils;
public class WaitingQueue<T>
{
    private readonly Transform[] _waitingPositions;
    private readonly StackList<T> _waitingEntities;
    
    public int Count => _waitingEntities.Count;

    public WaitingQueue(Transform[] queuePositions)
    {
        _waitingEntities = new StackList<T>();
        _waitingPositions = new Transform[queuePositions.Length];
        for (int position = 0; position < queuePositions.Length; position++)
        {
            _waitingPositions[position] = queuePositions[position];
        }
    }
    
    public void Add(T entity)
    {
        _waitingEntities.Push(entity);
    }
    
    public void Shift()
    {
        _waitingEntities.Shift();
    }

    public void Remove(T entity)
    {
        _waitingEntities.Remove(entity);
    }

    public T PeekFirst()
    {
        return _waitingEntities[0];
    }

    public Transform GetPosition(T entity)
    {
        int index = _waitingEntities.IndexOf(entity);
        return _waitingPositions[index];
    }

    public int GetPositionIndex(T entity)
    {
        return _waitingEntities.IndexOf(entity);
    }
}