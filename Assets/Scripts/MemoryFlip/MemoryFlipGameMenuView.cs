using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipGameMenuView : MonoBehaviour
{
    [SerializeField] private MemeoryFlipGameController _gameController;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    
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
        Disable();
    }

    private void OnEnable()
    {
        _gameController.OpenMenu += Enable;
        _continueButton.onClick.AddListener(ProcessContinueClicked);
        _restartButton.onClick.AddListener(ProcessRestartClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
    }

    private void OnDisable()
    {
        _gameController.OpenMenu -= Enable;
        _continueButton.onClick.RemoveListener(ProcessContinueClicked);
        _restartButton.onClick.RemoveListener(ProcessRestartClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
    }

    private void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    private void Disable()
    {
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
    
    private void ProcessMainMenuClicked()
    {
        MainMenuClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}
