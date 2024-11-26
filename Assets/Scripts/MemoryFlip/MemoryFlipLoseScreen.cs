using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipLoseScreen : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private AudioSource _loseSound;
    
    [SerializeField] private MemeoryFlipGameController _gameController;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action TryAgainClicked;
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
        _tryAgainButton.onClick.AddListener(ProcessTryAgainClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _gameController.GameLost += OpenScreen;
    }

    private void OnDisable()
    {
        _tryAgainButton.onClick.RemoveListener(ProcessTryAgainClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _gameController.GameLost -= OpenScreen;
    }
    
    private void OpenScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        SetValues();
        SaveLoseCount();
        _loseSound.Play();
    }

    private void SetValues()
    {
        _levelText.text = _gameController.View.DifficultyText;
    }

    private void ProcessMainMenuClicked()
    {
        MainMenuClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessTryAgainClicked()
    {
        TryAgainClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void SaveLoseCount()
    {
        if (PlayerPrefs.HasKey("MemoryFlipLoseCount"))
        {
           int savedLoseCount = PlayerPrefs.GetInt("MemoryFlipLoseCount");
           PlayerPrefs.SetInt("MemoryFlipLoseCount", savedLoseCount + 1);
        }
        else
        {
            PlayerPrefs.SetInt("MemoryFlipLoseCount", 1);
        }
        
        PlayerPrefs.Save();
    }
}
