using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipVictoryView : MonoBehaviour
{
    [SerializeField] private MemeoryFlipGameController _gameController;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private AudioSource _victorySound;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action RestartClicked;
    public event Action MainMenuClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _gameController.GameWon += OpenScreen;
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _gameController.GameWon -= OpenScreen;
    }

    private void ProcessMainMenuClicked()
    {
        _screenVisabilityHandler.DisableScreen();
        MainMenuClicked?.Invoke();
    }

    private void ProcessRestartButtonClicked()
    {
        _screenVisabilityHandler.DisableScreen();
        RestartClicked?.Invoke();
    }

    private void OpenScreen()
    {
        UpdateAmounts();
        _screenVisabilityHandler.EnableScreen();
        SaveVictoryCount();
        _victorySound.Play();
    }

    private void UpdateAmounts()
    {
        _timerText.text = _gameController.View.TimerText;
        _levelText.text = _gameController.View.DifficultyText;
    }
    
    private void SaveVictoryCount()
    {
        if (PlayerPrefs.HasKey("MemoryFlipWinCount"))
        {
            int savedLoseCount = PlayerPrefs.GetInt("MemoryFlipWinCount");
            PlayerPrefs.SetInt("MemoryFlipWinCount", savedLoseCount + 1);
        }
        else
        {
            PlayerPrefs.SetInt("MemoryFlipWinCount", 1);
        }
        
        PlayerPrefs.Save();
    }
}
