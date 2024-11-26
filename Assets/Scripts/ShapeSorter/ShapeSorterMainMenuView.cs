using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterMainMenuView : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action NewGameClicked;
    public event Action ContinueGameClicked;
    public event Action ExitClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(ProcessNewGameClicked);
        _continueButton.onClick.AddListener(ProcessContinueClicked);
        _exitButton.onClick.AddListener(ProcessExitClicked);
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(ProcessNewGameClicked);
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

    private void ProcessNewGameClicked()
    {
        NewGameClicked?.Invoke();
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
