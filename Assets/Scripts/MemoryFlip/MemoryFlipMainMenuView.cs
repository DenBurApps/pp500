using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipMainMenuView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action StartGameClicked;
    public event Action ContinueGameClicked;
    public event Action ExitClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(ProcessStartGameClicked);
        _continueButton.onClick.AddListener(ProcessContinueClicked);
        _exitButton.onClick.AddListener(ProcessExitClicked);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(ProcessStartGameClicked);
        _continueButton.onClick.RemoveListener(ProcessContinueClicked);
        _exitButton.onClick.RemoveListener(ProcessExitClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetContinueButtonStatus(bool status)
    {
        _continueButton.interactable = status;
    }

    private void ProcessStartGameClicked()
    {
        StartGameClicked?.Invoke();
    }

    private void ProcessExitClicked()
    {
        ExitClicked?.Invoke();
    }

    private void ProcessContinueClicked()
    {
        ContinueGameClicked?.Invoke();
    }
}
