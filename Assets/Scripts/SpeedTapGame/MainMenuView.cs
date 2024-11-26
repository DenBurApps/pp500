using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action NewGameButtonClicked;
    public event Action ContinueButtonClicked;
    public event Action ExitButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(ProcessNewGameButtonCLicked);
        _continueButton.onClick.AddListener(ProcessContinueButtonClicked);
        _exitButton.onClick.AddListener(ProcessExitButtonClicked);
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(ProcessNewGameButtonCLicked);
        _continueButton.onClick.RemoveListener(ProcessContinueButtonClicked);
        _exitButton.onClick.RemoveListener(ProcessExitButtonClicked);
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

    private void ProcessNewGameButtonCLicked()
    {
        NewGameButtonClicked?.Invoke();
    }

    private void ProcessContinueButtonClicked()
    {
        ContinueButtonClicked?.Invoke();
    }

    private void ProcessExitButtonClicked()
    {
        ExitButtonClicked?.Invoke();
    }
}
