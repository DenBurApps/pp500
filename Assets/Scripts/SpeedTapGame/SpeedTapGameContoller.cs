using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpeedTapGameContoller : MonoBehaviour
{
    private const float MaxTimerCount = 15f * 60;

    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private InGameMenuScreenView _inGameMenuScreenView;
    [SerializeField] private SpeedTapGameView _view;
    [SerializeField] private MeteoriteSpawner _meteoriteSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private int _spawnInterval = 2;
    [SerializeField] private LoseScreenView _loseScreen;
    [SerializeField] private VictoryScreenView _victoryScreen;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _meteorSound;
    [SerializeField] private AudioSource _bombSound;

    private int _health;
    private int _meteoriteCount;
    private int _bombCount;
    private float _currentTime;
    private IEnumerator _timerCoroutine;
    private IEnumerator _spawningCoroutine;

    public event Action GameWon;
    public event Action GameLost;
    public event Action MenuOpened;
    public event Action MainMenuOpened;
    public event Action NewGameStarted;

    public int Health => _health;

    public int MeteoriteCount => _meteoriteCount;

    public int BombCount => _bombCount;

    private void Start()
    {
        ResetAllValues();
        _view.Disable();
        _view.EnableAllHealthImages();
        _view.ResetAllValues();
    }

    private void OnEnable()
    {
        _view.MenuButtonClicked += OpenMenu;

        _meteoriteSpawner.ObjectClicked += ProcessObjectTap;
        _meteoriteSpawner.ObjectDied += ProcessObjectMissed;
        _bombSpawner.ObjectClicked += ProcessObjectTap;
        _bombSpawner.ObjectDied += ProcessObjectMissed;

        _inGameMenuScreenView.ContinueClicked += StartGame;
        _inGameMenuScreenView.RestartButtonClicked += StartNewGame;
        _inGameMenuScreenView.MainMenuClicked += ProcessMainMenuOpened;

        _mainMenu.StartNewGameClicked += StartNewGame;
        _mainMenu.ContinueGameClicked += StartGame;

        _loseScreen.TryAgainClicked += StartNewGame;
        _loseScreen.MainMenuClicked += ProcessMainMenuOpened;

        _victoryScreen.RestartClicked += StartNewGame;
        _victoryScreen.MainMenuClicked += ProcessMainMenuOpened;
    }

    private void OnDisable()
    {
        _view.MenuButtonClicked -= OpenMenu;

        _meteoriteSpawner.ObjectClicked -= ProcessObjectTap;
        _meteoriteSpawner.ObjectDied -= ProcessObjectMissed;
        _bombSpawner.ObjectClicked -= ProcessObjectTap;
        _bombSpawner.ObjectDied -= ProcessObjectMissed;

        _inGameMenuScreenView.ContinueClicked -= StartGame;
        _inGameMenuScreenView.RestartButtonClicked -= StartNewGame;
        _inGameMenuScreenView.MainMenuClicked -= ProcessMainMenuOpened;

        _mainMenu.StartNewGameClicked -= StartNewGame;
        _mainMenu.ContinueGameClicked -= StartGame;

        _loseScreen.TryAgainClicked -= StartNewGame;
        _loseScreen.MainMenuClicked -= ProcessMainMenuOpened;

        _victoryScreen.RestartClicked -= StartNewGame;
        _victoryScreen.MainMenuClicked -= ProcessMainMenuOpened;

        ResetAllValues();
        _view.ResetAllValues();
    }

    public bool CanContinueGame()
    {
        return _currentTime > 0f && _health > 0;
    }

    public string GetTimerText()
    {
        return _view.GetTimerText();
    }

    private void StartNewGame()
    {
        _currentTime = 0f;
        _health = 16;
        _meteoriteCount = 0;
        _bombCount = 0;
        _spawningCoroutine = null;
        _timerCoroutine = null;
        _view.ResetAllValues();

        StartGame();
        NewGameStarted?.Invoke();
    }

    private void StartGame()
    {
        _view.Enable();

        if (_timerCoroutine == null)
            _timerCoroutine = StartTimer();

        StartCoroutine(_timerCoroutine);

        if (_spawningCoroutine == null)
            _spawningCoroutine = StartSpawning();

        StartCoroutine(_spawningCoroutine);
        _gameMusic.Play();
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime < MaxTimerCount)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }

        ProcessGameWon();
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            int spawnChance = Random.Range(0, 2);

            if (spawnChance == 0)
                _bombSpawner.Spawn();
            else
                _meteoriteSpawner.Spawn();

            yield return interval;
        }
    }

    private void ProcessMainMenuOpened()
    {
        ResetAllValues();

        MainMenuOpened?.Invoke();
        _view.Disable();
        _gameMusic.Stop();
    }

    private void ResetAllValues()
    {
        _currentTime = 0f;
        _health = 16;
        _meteoriteCount = 0;
        _bombCount = 0;

        _timerCoroutine = null;
        _spawningCoroutine = null;
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
        SaveGameData();
        GameWon?.Invoke();
        StopGame();
    }

    private void ProcessGameLost()
    {
        SaveGameData();
        StopGame();
        _view.Disable();

        GameLost?.Invoke();
    }

    private void OpenMenu()
    {
        StopGame();
        _view.Disable();

        MenuOpened?.Invoke();
    }

    private void StopGame()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
        
        _gameMusic.Stop();
    }

    private void ProcessObjectTap(IInteractable interactableObject)
    {
        if (interactableObject == null)
            throw new ArgumentNullException(nameof(interactableObject));

        if (interactableObject is Meteorite)
        {
            IncreaceMeteoriteCount();
            _meteorSound.Play();
        }
        else if (interactableObject is Bomb)
        {
            DecreaceLife();
            _bombSound.Play();
        }
    }

    private void ProcessObjectMissed(IInteractable interactableObject)
    {
        if (interactableObject == null)
            throw new ArgumentNullException(nameof(interactableObject));

        if (interactableObject is Meteorite)
        {
            DecreaceLife();
        }
        else if (interactableObject is Bomb)
        {
            IncreaseBombCount();
        }
    }

    private void IncreaceMeteoriteCount()
    {
        _meteoriteCount++;
        _view.UpdateMeteoriteCount(_meteoriteCount);
    }

    private void IncreaseBombCount()
    {
        _bombCount++;
        _view.UpdateBombCount(_bombCount);
    }

    private void DecreaceLife()
    {
        _health--;
        _view.DecreaceHpImage(_health);

        if (_health <= 0)
        {
            _health = 0;
            _view.DecreaceHpImage(_health);
            ProcessGameLost();
        }
    }

    private void SaveGameData()
    {
        float savedMaxTime = PlayerPrefs.GetFloat("SpeedTapMaxSurvivalTime", 0f);
        if (_currentTime > savedMaxTime)
        {
            PlayerPrefs.SetFloat("SpeedTapMaxSurvivalTime", _currentTime);
            PlayerPrefs.SetString("SpeedTapMaxSurvivalTimeText", GetTimerText());
        }

        int savedMeteorites = PlayerPrefs.GetInt("TotalMeteoritesDestroyed", 0);
        PlayerPrefs.SetInt("TotalMeteoritesDestroyed", savedMeteorites + _meteoriteCount);

        PlayerPrefs.Save();
    }
}