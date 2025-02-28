using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _buttonOffset = 50f;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _newGameInitialPosition;
    private Vector3 _continueInitialPosition;
    private Vector3 _exitInitialPosition;
    
    public event Action NewGameButtonClicked;
    public event Action ContinueButtonClicked;
    public event Action ExitButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        _newGameInitialPosition = _newGameButton.transform.position;
        _continueInitialPosition = _continueButton.transform.position;
        _exitInitialPosition = _exitButton.transform.position;
    }

    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(ProcessNewGameButtonClicked);
        _continueButton.onClick.AddListener(ProcessContinueButtonClicked);
        _exitButton.onClick.AddListener(ProcessExitButtonClicked);
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(ProcessNewGameButtonClicked);
        _continueButton.onClick.RemoveListener(ProcessContinueButtonClicked);
        _exitButton.onClick.RemoveListener(ProcessExitButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        PlayEntranceAnimation();
    }

    public void Disable()
    {
        PlayExitAnimation(() => _screenVisabilityHandler.DisableScreen());
    }

    public void SetContinueButtonStatus(bool status)
    {
        _continueButton.interactable = status;
        
        if (status)
        {
            _continueButton.transform.DOScale(1f, _animationDuration / 2);
        }
        else
        {
            _continueButton.transform.DOScale(0.9f, _animationDuration / 2);
        }
    }

    private void ProcessNewGameButtonClicked()
    {
        _newGameButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        NewGameButtonClicked?.Invoke();
    }

    private void ProcessExitButtonClicked()
    {
        _exitButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        ExitButtonClicked?.Invoke();
    }

    private void ProcessContinueButtonClicked()
    {
        _continueButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        ContinueButtonClicked?.Invoke();
    }
    
    private void PlayEntranceAnimation()
    {
        _newGameButton.transform.position = _newGameInitialPosition + Vector3.left * _buttonOffset;
        _continueButton.transform.position = _continueInitialPosition + Vector3.left * _buttonOffset;
        _exitButton.transform.position = _exitInitialPosition + Vector3.left * _buttonOffset;
        
        _newGameButton.transform.DOMove(_newGameInitialPosition, _animationDuration).SetEase(Ease.OutBack);
        _continueButton.transform.DOMove(_continueInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_animationDuration * 0.2f);
        _exitButton.transform.DOMove(_exitInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_animationDuration * 0.4f);
    }
    
    private void PlayExitAnimation(Action onComplete)
    {
        _newGameButton.transform.DOMove(_newGameInitialPosition + Vector3.right * _buttonOffset, _animationDuration).SetEase(Ease.InBack);
        _continueButton.transform.DOMove(_continueInitialPosition + Vector3.right * _buttonOffset, _animationDuration).SetEase(Ease.InBack).SetDelay(_animationDuration * 0.2f);
        _exitButton.transform.DOMove(_exitInitialPosition + Vector3.right * _buttonOffset, _animationDuration).SetEase(Ease.InBack).SetDelay(_animationDuration * 0.4f).OnComplete(() => onComplete?.Invoke());
    }
}