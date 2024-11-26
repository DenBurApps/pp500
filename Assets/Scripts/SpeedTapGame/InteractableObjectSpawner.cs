using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectSpawner : ObjectPool<InteractableObject>
{
    [SerializeField] private InteractableObject _prefab;
    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;

    public event Action<IInteractable> ObjectDied;
    public event Action<IInteractable> ObjectClicked;

    private List<InteractableObject> _spawnedObjects = new List<InteractableObject>();

    private void Awake()
    {
        Initalize(_prefab);
    }

    private void OnEnable()
    {
        _speedTapGameContoller.NewGameStarted += ReturnAllObjectsToPool;
        _speedTapGameContoller.GameWon += ReturnAllObjectsToPool;
        _speedTapGameContoller.MainMenuOpened += ReturnAllObjectsToPool;
        _speedTapGameContoller.GameLost += ReturnAllObjectsToPool;
    }

    private void OnDisable()
    {
        _speedTapGameContoller.NewGameStarted -= ReturnAllObjectsToPool;
        _speedTapGameContoller.GameWon -= ReturnAllObjectsToPool;
        _speedTapGameContoller.MainMenuOpened -= ReturnAllObjectsToPool;
        _speedTapGameContoller.GameLost -= ReturnAllObjectsToPool;
    }

    public void Spawn()
    {
        if (ActiveObjects.Count >= Capacity)
            return;

        if (TryGetObject(out InteractableObject @object, _prefab))
        {
            @object.transform.position = _spawnArea.GetRandomPositionToSpawn();
            @object.LifetimeEnded += ProcessObjectDied;
            @object.GotClicked += ProcessObjectClicked;
            _spawnedObjects.Add(@object);
        }
    }

    public void ReturnToPull(InteractableObject @object)
    {
        if (@object == null)
           return;

        @object.LifetimeEnded -= ProcessObjectDied;
        @object.GotClicked -= ProcessObjectClicked;
        PutObject(@object);

        if (_spawnedObjects.Contains(@object))
            _spawnedObjects.Remove(@object);
    }

    public void ReturnAllObjectsToPool()
    {
        if (_spawnedObjects.Count <= 0)
            return;

        List<InteractableObject> objectsToReturn = new List<InteractableObject>(_spawnedObjects);
        foreach (var @object in objectsToReturn)
        {
            ReturnToPull(@object);
        }
    }

    private void ProcessObjectDied(InteractableObject @object)
    {
        ObjectDied?.Invoke(@object);
        ReturnToPull(@object);
    }

    private void ProcessObjectClicked(InteractableObject @object)
    {
        ObjectClicked?.Invoke(@object);
        ReturnToPull(@object);
    }
}