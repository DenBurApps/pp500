using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeSorterGameController : MonoBehaviour
{
    private const float MaxTimerCount = 5f * 60;
    private const int SquareValue = 20;
    private const int CircleValue = 75;
    private const int TriangleValue = 50;
    private const int RectangleValue = 100;

    [SerializeField] private ShapeSorterDifficultySelection _difficultySelection;
    [SerializeField] private ShapeSorterGameView _view;
    [SerializeField] private ShapeSorterGameMenuView _gameMenu;
    [SerializeField] private ShapeSorterMainMenu _mainMenu;
    [SerializeField] private ShapeSorterVictoryScreen _victoryScreen;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private FigureSlot[] _figureSlots;
    [SerializeField] private FigureSpawner[] _figureSpawners;
    [SerializeField] private float _spawnInterval = 2f;

    private int _coins;
    private int _squareAmount;
    private int _circleAmount;
    private int _triangleAmount;
    private int _rectangleAmount;
    private FigureSlot _currentFigureSlot;
    private DifficultyTypes _currentDificulty;

    private float _currentTime;
    private IEnumerator _timerCoroutine;
    private IEnumerator _spawnCoroutine;

    public event Action GameWon;
    public event Action MenuOpen;
    public event Action MainMenuOpened;
    public event Action GameRestarted;

    public ShapeSorterGameView View => _view;

    private void Start()
    {
        _view.Disable();
        ReturnDefaultValues();
    }

    private void OnEnable()
    {
        _difficultySelection.EasySelected += () => StartNewGame(DifficultyTypes.Easy);
        _difficultySelection.NormalSelected += () => StartNewGame(DifficultyTypes.Normal);
        _difficultySelection.HardSelected += () => StartNewGame(DifficultyTypes.Hard);
        _view.MenuButtonClicked += ProcessMenuOpen;

        _gameMenu.ContinueClicked += ContinueGame;
        _gameMenu.RestartButtonClicked += ProcessGameRestart;
        _gameMenu.MainMenuClicked += ProcessMainMenuOpened;

        _mainMenu.ContinueGame += ContinueGame;

        _victoryScreen.RestartClicked += ProcessGameRestart;
        _victoryScreen.MainMenuClicked += ProcessMainMenuOpened;
    }

    private void OnDisable()
    {
        _difficultySelection.EasySelected -= () => StartNewGame(DifficultyTypes.Easy);
        _difficultySelection.NormalSelected -= () => StartNewGame(DifficultyTypes.Normal);
        _difficultySelection.HardSelected -= () => StartNewGame(DifficultyTypes.Hard);
        _view.MenuButtonClicked -= ProcessMenuOpen;
        
        _gameMenu.ContinueClicked -= ContinueGame;
        _gameMenu.RestartButtonClicked -= ProcessGameRestart;
        _gameMenu.MainMenuClicked -= ProcessMainMenuOpened;
        
        _mainMenu.ContinueGame -= ContinueGame;
        
        _victoryScreen.RestartClicked -= ProcessGameRestart;
        _victoryScreen.MainMenuClicked -= ProcessMainMenuOpened;
        
        foreach (var spawner in _figureSpawners)
        {
            spawner.FigureCorrectlyPlaced -= ProcessFigureCorrectlyPlaced;
        }
    }

    public bool CanContinueGame()
    {
        return _currentTime > 0f;
    }
    
    private void ProcessGameRestart()
    {
        GameRestarted?.Invoke();
        ReturnDefaultValues();
        _view.Disable();
        _backgroundMusic.Stop();
    }
    
    private void ProcessMainMenuOpened()
    {
        MainMenuOpened?.Invoke();
        _view.Disable();
        _backgroundMusic.Stop();
    }
    
    private void ProcessMenuOpen()
    {
        MenuOpen?.Invoke();
        _view.Disable();
        StopGame();
    }

    private void StopGame()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        
        if(_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
        
        _backgroundMusic.Stop();
    }

    private void ContinueGame()
    {
        if (_timerCoroutine != null)
            StartCoroutine(_timerCoroutine);
        
        if(_spawnCoroutine != null)
            StartCoroutine(_spawnCoroutine);
        
        _view.Enable();
        _backgroundMusic.Play();
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            int randomSpanerIndex = Random.Range(0, _figureSpawners.Length);

            _figureSpawners[randomSpanerIndex].Spawn();

            yield return interval;
        }
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime <= MaxTimerCount)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }

        ProcessGameWon();
    }

    private void UpdateTimer(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _view.SetTimerValue(minutes, seconds);
    }

    private void ProcessGameWon()
    {
        GameWon?.Invoke();
        
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = null;
        _timerCoroutine = null;
        
        _view.Disable();
        SaveDifficultyPassed();
        SaveProgress();
        _backgroundMusic.Stop();
    }

    private void ReturnDefaultValues()
    {
        _coins = 0;
        _squareAmount = 0;
        _circleAmount = 0;
        _triangleAmount = 0;
        _rectangleAmount = 0;
        _currentTime = 0;
        
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = null;
        _timerCoroutine = null;

        foreach (var slot in _figureSlots)
        {
            slot.ToggleActiveStatus(false);
        }

        SetViewValues();
        
        foreach (var spawner in _figureSpawners)
        {
            spawner.ReturnAllObjectsToPool();
        }
        
        _backgroundMusic.Stop();
    }

    private void SetViewValues()
    {
        _view.SetTextAmount(_coins, _view.CoinsAmount);
        _view.SetTextAmount(_squareAmount, _view.SquareQuantity);
        _view.SetTextAmount(_circleAmount, _view.CircleQuantity);
        _view.SetTextAmount(_triangleAmount, _view.TriangleQuantity);
        _view.SetTextAmount(_rectangleAmount, _view.RectangleQuantity);
    }

    private void EnableRandomFigureSlot()
    {
        int randomIndex = Random.Range(0, _figureSlots.Length);

        if (_currentFigureSlot == null)
        {
            _currentFigureSlot = _figureSlots[randomIndex];
            _currentFigureSlot.ToggleActiveStatus(true);
        }
        else
        {
            _currentFigureSlot.ToggleActiveStatus(false);
            _currentFigureSlot = _figureSlots[randomIndex];
            _currentFigureSlot.ToggleActiveStatus(true);
        }
    }

    private void StartNewGame(DifficultyTypes difficultyType)
    {
        _view.Enable();
        ReturnDefaultValues();

        _currentDificulty = difficultyType;

        foreach (var spawner in _figureSpawners)
        {
            spawner.SetDifficulty(difficultyType);
            spawner.FigureCorrectlyPlaced += ProcessFigureCorrectlyPlaced;
        }

        if (_timerCoroutine == null)
        {
            _timerCoroutine = StartTimer();
            StartCoroutine(_timerCoroutine);
        }

        if (_spawnCoroutine == null)
        {
            _spawnCoroutine = StartSpawning();
            StartCoroutine(_spawnCoroutine);
        }

        EnableRandomFigureSlot();
        _backgroundMusic.Play();
    }

    private void ProcessFigureCorrectlyPlaced(FigureTypes type)
    {
        int value = 0;
        int amount = 0;

        switch (type)
        {
            case FigureTypes.Cube:
                value = SquareValue;
                amount = ++_squareAmount;
                _view.SetTextAmount(amount, _view.SquareQuantity);
                break;
            case FigureTypes.Circle:
                value = CircleValue;
                amount = ++_circleAmount;
                _view.SetTextAmount(amount, _view.CircleQuantity);
                break;
            case FigureTypes.Triangle:
                value = TriangleValue;
                amount = ++_triangleAmount;
                _view.SetTextAmount(amount, _view.TriangleQuantity);
                break;
            case FigureTypes.Rectangle:
                value = RectangleValue;
                amount = ++_rectangleAmount;
                _view.SetTextAmount(amount, _view.RectangleQuantity);
                break;
        }

        _correctSound.Play();
        UpdateCoinsAndSlot(value);
    }
    
    private void UpdateCoinsAndSlot(int value)
    {
        _coins += value;
        _view.SetTextAmount(_coins, _view.CoinsAmount);
        EnableRandomFigureSlot();
    }
    
    private void SaveProgress()
    {
        if (PlayerPrefs.HasKey("ShapeSorterTotalCoins"))
        {
            int currentCoins = PlayerPrefs.GetInt("ShapeSorterTotalCoins");
            PlayerPrefs.SetInt("ShapeSorterTotalCoins", currentCoins + _coins);
        }
        else
        {
            PlayerPrefs.SetInt("ShapeSorterTotalCoins", _coins);
        }
        
        if (PlayerPrefs.HasKey("ShapeSorterTotalTime"))
        {
            float currentTime = PlayerPrefs.GetFloat("ShapeSorterTotalTime");
            PlayerPrefs.SetFloat("ShapeSorterTotalTime", _currentTime + currentTime);
        }
        else
        {
            PlayerPrefs.SetFloat("ShapeSorterTotalTime", _currentTime);
        }
    }
    
    private void SaveDifficultyPassed()
    {
        if (_currentDificulty == DifficultyTypes.Easy)
        {
            ValidateSaveKey("ShapeSorterEasyPassed");
        }
        else if (_currentDificulty == DifficultyTypes.Normal)
        {
            ValidateSaveKey("ShapeSorterNormalPassed");
        }
        else if (_currentDificulty == DifficultyTypes.Hard)
        {
            ValidateSaveKey("ShapeSorterHardPassed");
        }

        PlayerPrefs.Save();
    }
    
    private static void ValidateSaveKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int currentPassedValue = PlayerPrefs.GetInt(key);
            PlayerPrefs.SetInt(key, currentPassedValue + 1);
        }
        else
        {
            PlayerPrefs.SetInt(key, 1);
        }
    }
}

public enum DifficultyTypes
{
    Easy,
    Normal,
    Hard
}