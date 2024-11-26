using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class InGameMenuScreenView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MainMenuClicked;
    public event Action RestartButtonClicked;
    public event Action ContinueClicked;
    
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
        _speedTapGameContoller.MenuOpened += EnableScreen;
        _continueButton.onClick.AddListener(ProcessContinueClicked);
        _restartButton.onClick.AddListener(ProcessRestartClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
    }

    private void OnDisable()
    {
        _speedTapGameContoller.MenuOpened -= EnableScreen;
        _continueButton.onClick.RemoveListener(ProcessContinueClicked);
        _restartButton.onClick.RemoveListener(ProcessRestartClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
    }

    private void ProcessMainMenuClicked()
    {
        MainMenuClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessRestartClicked()
    {
        RestartButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessContinueClicked()
    {
        ContinueClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }
}
