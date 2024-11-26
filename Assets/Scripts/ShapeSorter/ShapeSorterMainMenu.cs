using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeSorterMainMenu : MonoBehaviour
{
    [SerializeField] private ShapeSorterMainMenuView _view;
    [SerializeField] private ShapeSorterDifficultySelection _dificultySelection;
    [SerializeField] private ShapeSorterGameController _gameController;
    
    public event Action NewGame;
    public event Action ContinueGame;
    
    private void Start()
    {
        ProcessScreenOpen();
    }
    
    private void OnEnable()
    {
        _view.NewGameClicked += ProcessGameStart;
        _view.ContinueGameClicked += ProcessGameContinue;
        _view.ExitClicked += ProcessExit;
        _dificultySelection.BackButtonClicked += ProcessScreenOpen;
        _gameController.MainMenuOpened += ProcessScreenOpen;
        SetContinueButtonStatus();
    }

    private void OnDisable()
    {
        _view.NewGameClicked -= ProcessGameStart;
        _view.ContinueGameClicked -= ProcessGameContinue;
        _view.ExitClicked -= ProcessExit; 
        _dificultySelection.BackButtonClicked -= ProcessScreenOpen;
        _gameController.MainMenuOpened -= ProcessScreenOpen;
        
    }
    
    private void ProcessGameStart()
    {
        NewGame?.Invoke();
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
