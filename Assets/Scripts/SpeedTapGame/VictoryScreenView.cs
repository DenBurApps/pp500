using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class VictoryScreenView : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _meteoriteText;
    [SerializeField] private TMP_Text _bombText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private AudioSource _winSound;

    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;
    
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
        _speedTapGameContoller.GameWon += OpenScreen;
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameWon -= OpenScreen;
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
        _winSound.Play();
    }

    private void UpdateAmounts()
    {
        _timerText.text = _speedTapGameContoller.GetTimerText();
        _meteoriteText.text = _speedTapGameContoller.MeteoriteCount.ToString();
        _bombText.text = _speedTapGameContoller.BombCount.ToString();
        _hpText.text = _speedTapGameContoller.Health + " hp";
    }
}
