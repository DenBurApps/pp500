using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;
    [SerializeField] private GameObject _parent;
    
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly List<T> _activeObjects = new List<T>();

    public IReadOnlyCollection<T> ActiveObjects => _activeObjects;
    public int Capacity => _capacity;

    protected void Initalize(T prefab)
    {
        for (int i = 0; i < _capacity; i++)
        {
            T spawnedObject = Instantiate(prefab, _container.transform.position, Quaternion.identity,_parent.transform);
            spawnedObject.gameObject.SetActive(false);

            _queue.Enqueue(spawnedObject);
        }
    }

    protected bool TryGetObject(out T @object, T prefab)
    {
        if (_queue.Count > 0)
        {
            @object = _queue.Dequeue();
            _activeObjects.Add(@object);
            @object.gameObject.SetActive(true);
            return true;
        }

        @object = Instantiate(prefab);
        return true;
    }

    protected void PutObject(T @object)
    {
        if (@object == null)
            throw new ArgumentNullException(nameof(@object));

        @object.transform.position = _container.transform.position;
        @object.gameObject.SetActive(false);
        _activeObjects.Remove(@object);
        _queue.Enqueue(@object);
    }
}
