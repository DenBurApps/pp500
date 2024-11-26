using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryFlipMainMenu : MonoBehaviour
{
    [SerializeField] private MemoryFlipMainMenuView _view;
    [SerializeField] private MemoryFlipDificultySelection _dificultySelection;
    [SerializeField] private MemeoryFlipGameController _gameController;

    public event Action StartGame;
    public event Action ContinueGame;
    
    private void Start()
    {
        _view.Enable();
    }

    private void OnEnable()
    {
        _view.StartGameClicked += ProcessGameStart;
        _view.ContinueGameClicked += ProcessGameContinue;
        _view.ExitClicked += ProcessExit;
        _dificultySelection.BackButtonClicked += ProcessScreenOpen;
        _gameController.MainMenuOpened += ProcessScreenOpen;
        SetContinueButtonStatus();
    }

    private void OnDisable()
    {
        _view.StartGameClicked -= ProcessGameStart;
        _view.ContinueGameClicked -= ProcessGameContinue;
        _view.ExitClicked -= ProcessExit;
        _dificultySelection.BackButtonClicked -= ProcessScreenOpen;
        _gameController.MainMenuOpened -= ProcessScreenOpen;
        
    }

    private void ProcessGameStart()
    {
        StartGame?.Invoke();
        _view.Disable();
    }

    private void ProcessGameContinue()
    {
        ContinueGame?.Invoke();
        _view.Disable();
    }

    private void ProcessExit()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void ProcessScreenOpen()
    {
        _view.Enable();
        SetContinueButtonStatus();
    }

    private void SetContinueButtonStatus()
    {
        _view.SetContinueButtonStatus(_gameController.CanContinueGame());
    }
}
