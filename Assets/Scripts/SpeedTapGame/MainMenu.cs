using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MainMenuView _view;
    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;

    public event Action StartNewGameClicked;
    public event Action ContinueGameClicked;

    private void Start()
    {
        ProcessMainMenuOpened();
    }

    private void OnEnable()
    {
        _view.NewGameButtonClicked += StartNewGame;
        _view.ContinueButtonClicked += ContinueGame;
        _view.ExitButtonClicked += Exit;

        _speedTapGameContoller.MainMenuOpened += ProcessMainMenuOpened;
        
    }

    private void OnDisable()
    {
        _view.NewGameButtonClicked -= StartNewGame;
        _view.ContinueButtonClicked -= ContinueGame;
        _view.ExitButtonClicked -= Exit;

        _speedTapGameContoller.MainMenuOpened -= ProcessMainMenuOpened;
    }


    private void StartNewGame()
    {
        StartNewGameClicked?.Invoke();
        _view.Disable();
    }

    private void ContinueGame()
    {
        ContinueGameClicked?.Invoke();
        _view.Disable();
    }

    private void Exit()
    {
        //Сохраняем прогресс игрока 
        //Ресетим игру
        SceneManager.LoadScene("MainScene");
    }

    private void SetContinueButtonVisability()
    {
        _view.SetContinueButtonStatus(_speedTapGameContoller.CanContinueGame());
    }

    private void ProcessMainMenuOpened()
    {
        _view.Enable();
        SetContinueButtonVisability();
    }
}
