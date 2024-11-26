using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FigureSpawner : ObjectPool<Figure>
{
    [SerializeField] private ShapeSorterSpawnArea _spawnArea;
    [SerializeField] private float _yPosition;
    [SerializeField] private Figure _prefab;
    [SerializeField] private FigureSlot _figureSlot;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private FigureTypes _figureTypeToSpawn;

    private DifficultyTypes _currentDifficulty;
    private List<Figure> _spawnedObjects = new List<Figure>();

    public event Action<FigureTypes> FigureCorrectlyPlaced;
    
    public FigureTypes FigureTypeToSpawn => _figureTypeToSpawn;

    private void Awake()
    {
        Initalize(_prefab);
    }

    private void OnEnable()
    {
        _figureSlot.FigureInsertedIncorrectly += ProcessFigureIncorrectlyPlaced;
        _figureSlot.FigureInsertedCorrectly += ProcessFigureCorrectlyPlaced;
    }

    private void OnDisable()
    {
        _figureSlot.FigureInsertedIncorrectly -= ProcessFigureIncorrectlyPlaced;
        _figureSlot.FigureInsertedCorrectly -= ProcessFigureCorrectlyPlaced;
    }

    public void SetDifficulty(DifficultyTypes difficulty)
    {
        _currentDifficulty = difficulty;
    }

    public void Spawn()
    {
        if (ActiveObjects.Count >= Capacity)
            return;


        if (TryGetObject(out Figure figure, _prefab))
        {
            Vector2 randomSpawnPosition = _spawnArea.GetRandomXPositionToSpawn();
            figure.SetParentCanvas(_canvas);

            RectTransform rectTransform = figure.RectTransform;
            rectTransform.anchoredPosition = randomSpawnPosition;

            figure.SetDifficultySpeed(_currentDifficulty);
            figure.ReachedBottom += ReturnToPull;
            figure.EnableMovement();

            _spawnedObjects.Add(figure);
        }
    }

    public void ReturnToPull(Figure figure)
    {
        if (figure == null)
            return;

        figure.ReachedBottom -= ReturnToPull;
        PutObject(figure);

        if (_spawnedObjects.Contains(figure))
            _spawnedObjects.Remove(figure);
    }

    public void ReturnAllObjectsToPool()
    {
        if (_spawnedObjects.Count <= 0)
            return;

        List<Figure> objectsToReturn = new List<Figure>(_spawnedObjects);
        foreach (var figure in objectsToReturn)
        {
            figure.DisableMovement();
            ReturnToPull(figure);
        }
    }

    private void ProcessFigureCorrectlyPlaced(Figure figure)
    {
        FigureCorrectlyPlaced?.Invoke(_figureTypeToSpawn);
        ReturnToPull(figure);
    }
    
    private void ProcessFigureIncorrectlyPlaced(Figure figure)
    {
        figure.ReturnToPreviousPosition();
    }
}

public enum FigureTypes
{
    Cube,
    Circle,
    Triangle,
    Rectangle
}