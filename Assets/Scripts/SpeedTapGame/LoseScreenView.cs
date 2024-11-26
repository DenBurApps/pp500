using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class LoseScreenView : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _meteoriteText;
    [SerializeField] private TMP_Text _bombText;
    [SerializeField] private AudioSource _loseSound;

    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;
    
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
        _speedTapGameContoller.GameLost += OpenScreen;
    }

    private void OnDisable()
    {
        _tryAgainButton.onClick.RemoveListener(ProcessTryAgainClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameLost -= OpenScreen;
    }

    private void OpenScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        SetValues();
        _loseSound.Play();
    }

    private void SetValues()
    {
        _timerText.text = _speedTapGameContoller.GetTimerText();
        _meteoriteText.text = _speedTapGameContoller.MeteoriteCount.ToString();
        _bombText.text = _speedTapGameContoller.BombCount.ToString();
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
}
