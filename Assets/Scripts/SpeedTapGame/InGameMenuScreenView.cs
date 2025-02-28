using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class InGameMenuScreenView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;

    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _buttonOffset = 50f;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private Vector3 _continueButtonInitialPosition;
    private Vector3 _restartButtonInitialPosition;
    private Vector3 _mainMenuButtonInitialPosition;

    public event Action MainMenuClicked;
    public event Action RestartButtonClicked;
    public event Action ContinueClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();

        _continueButtonInitialPosition = _continueButton.transform.position;
        _restartButtonInitialPosition = _restartButton.transform.position;
        _mainMenuButtonInitialPosition = _mainMenuButton.transform.position;
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
        _mainMenuButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        MainMenuClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessRestartClicked()
    {
        _restartButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        RestartButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessContinueClicked()
    {
        _continueButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        ContinueClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        PlayEntranceAnimation();
    }

    private void PlayEntranceAnimation()
    {
        _continueButton.transform.position = _continueButtonInitialPosition + Vector3.left * _buttonOffset;
        _restartButton.transform.position = _restartButtonInitialPosition + Vector3.right * _buttonOffset;
        _mainMenuButton.transform.position = _mainMenuButtonInitialPosition + Vector3.down * _buttonOffset;

        _continueButton.transform.DOMove(_continueButtonInitialPosition, _animationDuration).SetEase(Ease.OutBack);
        _restartButton.transform.DOMove(_restartButtonInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_animationDuration * 0.2f);
        _mainMenuButton.transform.DOMove(_mainMenuButtonInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_animationDuration * 0.4f);
    }
}