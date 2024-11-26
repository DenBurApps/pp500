using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeoryFlipGameController : MonoBehaviour
{
    private const string EasyDifficultyText = "Easy";
    private const string NormalDifficultyText = "Normal";
    private const string HardDifficultyText = "Hard";
    private const float EasyTimerValue = 5f * 60;
    private const float NormalTimerValue = 3f * 60;
    private const float HardTimerValue = 1f * 60;

    [SerializeField] private MemoryFlipGameView _view;
    [SerializeField] private MemoryFlipDificultySelection _dificultySelection;
    [SerializeField] private FishCell[] _fishCells;
    [SerializeField] private MemoryFlipGameMenuView _gameMenu;
    [SerializeField] private MemoryFlipVictoryView _victoryScreen;
    [SerializeField] private MemoryFlipLoseScreen _loseScreen;
    [SerializeField] private MemoryFlipMainMenu _mainMenu;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _fishFlipSound;

    private float _currentTime;
    private IEnumerator _timerCoroutine;
    private FishCell _firstFish;
    private FishCell _secondFish;
    private int _fishPairs;
    private DifficultyOptions _currentDifficulty;
    private FishCellTypeProvider _fishCellTypeProvider;

    public event Action OpenMenu;
    public event Action GameWon;
    public event Action GameLost;
    public event Action GameRestarted;
    public event Action MainMenuOpened;

    public MemoryFlipGameView View => _view;

    private void Awake()
    {
        _fishCellTypeProvider = new FishCellTypeProvider();
    }

    private void Start()
    {
        _view.Disable();
        ResetDefaultValues();
    }

    private void OnEnable()
    {
        _view.MenuButtonClicked += ProcessMenuOpen;
        _dificultySelection.EasySelected += () =>
            ProcessNewGameStart(EasyTimerValue, EasyDifficultyText, DifficultyOptions.Easy);
        _dificultySelection.NormalSelected += () =>
            ProcessNewGameStart(NormalTimerValue, NormalDifficultyText, DifficultyOptions.Normal);
        _dificultySelection.HardSelected += () =>
            ProcessNewGameStart(HardTimerValue, HardDifficultyText, DifficultyOptions.Hard);

        _gameMenu.ContinueClicked += ContinueGame;
        _gameMenu.RestartButtonClicked += ProcessGameRestart;
        _gameMenu.MainMenuClicked += ProcessMainMenuOpened;

        _victoryScreen.MainMenuClicked += ProcessMainMenuOpened;
        _victoryScreen.RestartClicked += ProcessGameRestart;

        _loseScreen.MainMenuClicked += ProcessMainMenuOpened;
        _loseScreen.TryAgainClicked += ProcessGameRestart;

        _mainMenu.ContinueGame += ContinueGame;
        
        foreach (var cell in _fishCells)
        {
            cell.Clicked += ProcessFishClicked;
        }
    }

    private void OnDisable()
    {
        _view.MenuButtonClicked -= ProcessMenuOpen;
        _dificultySelection.EasySelected -= () =>
            ProcessNewGameStart(EasyTimerValue, EasyDifficultyText, DifficultyOptions.Easy);
        _dificultySelection.NormalSelected -= () =>
            ProcessNewGameStart(NormalTimerValue, NormalDifficultyText, DifficultyOptions.Normal);
        _dificultySelection.HardSelected -= () =>
            ProcessNewGameStart(HardTimerValue, HardDifficultyText, DifficultyOptions.Hard);

        _gameMenu.ContinueClicked -= ContinueGame;
        _gameMenu.RestartButtonClicked -= ProcessGameRestart;
        _gameMenu.MainMenuClicked -= ProcessMainMenuOpened;

        _victoryScreen.MainMenuClicked -= ProcessMainMenuOpened;
        _victoryScreen.RestartClicked -= ProcessGameRestart;

        _loseScreen.MainMenuClicked -= ProcessMainMenuOpened;
        _loseScreen.TryAgainClicked -= ProcessGameRestart;

        _mainMenu.ContinueGame -= ContinueGame;
    }

    public bool CanContinueGame()
    {
        return _currentTime > 0f && _fishPairs > 0;
    }

    private void ProcessMainMenuOpened()
    {
        MainMenuOpened?.Invoke();
        _view.Disable();
    }

    private void ProcessFishClicked(FishCell cell)
    {
        if (cell.IsFliped)
            return;

        if (_firstFish != null && _secondFish != null)
        {
            _firstFish.HideFishImage();
            _secondFish.HideFishImage();

            _firstFish = null;
            _secondFish = null;
        }

        if (_firstFish == null)
        {
            _firstFish = cell;
            _firstFish.ShowFishImage();
            _fishFlipSound.Play();
            return;
        }

        if (_secondFish == null)
        {
            _secondFish = cell;
            _secondFish.ShowFishImage();
            _fishFlipSound.Play();
        }

        if (_firstFish != null && _secondFish != null)
            CompareChoseFishes();
    }

    private void CompareChoseFishes()
    {
        if (_firstFish.CurrentType == _secondFish.CurrentType)
        {
            _fishPairs--;

            _firstFish.Disable();
            _secondFish.Disable();

            if (_fishPairs <= 0)
            {
                ProcessGameWon();
            }

            _firstFish = null;
            _secondFish = null;
        }
    }

    private void ProcessNewGameStart(float maxTimerValue, string difficultyText, DifficultyOptions difficulty)
    {
        _view.Enable();
        ResetDefaultValues();

        _timerCoroutine = StartTimer(maxTimerValue);
        StartCoroutine(_timerCoroutine);
        _view.SetDifficultyText(difficultyText);

        _currentDifficulty = difficulty;

        List<FishTypes> fishList = _fishCellTypeProvider.GetFishPairs();

        for (int i = 0; i < fishList.Count; i++)
        {
            _fishCells[i].SetRandomFishType(fishList[i]);
        }
        
        _backgroundMusic.Play();
    }

    private IEnumerator StartTimer(float maxTime)
    {
        _currentTime = maxTime;

        while (_currentTime >= 0f)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }

        ProcessGameLost();
    }

    private void UpdateTimer(float time)
    {
        time = Mathf.Max(time, 0);

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _view.SetTimerValue(minutes, seconds);
    }

    private void ProcessGameRestart()
    {
        GameRestarted?.Invoke();
        ResetDefaultValues();
        _view.Disable();
        _backgroundMusic.Stop();
    }

    private void StopGame()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        
        _backgroundMusic.Stop();
    }

    private void ContinueGame()
    {
        if (_timerCoroutine != null)
            StartCoroutine(_timerCoroutine);

        _view.Enable();
        _backgroundMusic.Play();
    }

    private void ProcessGameWon()
    {
        GameWon?.Invoke();
        _view.Disable();
        SaveDifficultyPassed();
        ResetDefaultValues();
    }

    private void ProcessGameLost()
    {
        GameLost?.Invoke();
        _view.Disable();
        ResetDefaultValues();
    }

    private void ProcessMenuOpen()
    {
        OpenMenu?.Invoke();
        _view.Disable();
        StopGame();
    }

    private void ResetDefaultValues()
    {
        _currentTime = 0;
        _fishPairs = 10;

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        foreach (var cell in _fishCells)
        {
            cell.ReturnToDefault();
        }

        _timerCoroutine = null;

        _firstFish = null;
        _secondFish = null;
        
        _currentDifficulty = DifficultyOptions.Empty;
        _backgroundMusic.Stop();
    }

    private void SaveDifficultyPassed()
    {
        if (_currentDifficulty == DifficultyOptions.Easy)
        {
            ValidateSaveKey("MemoryFlipEasyPassed");
        }
        else if (_currentDifficulty == DifficultyOptions.Normal)
        {
            ValidateSaveKey("MemoryFlipNormalPassed");
        }
        else if (_currentDifficulty == DifficultyOptions.Hard)
        {
            ValidateSaveKey("MemoryFlipHardPassed");
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


public enum DifficultyOptions
{
    Easy,
    Normal,
    Hard,
    Empty
}