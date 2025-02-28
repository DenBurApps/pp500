using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class VictoryScreenView : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _meteoriteText;
    [SerializeField] private TMP_Text _bombText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private AudioSource _winSound;

    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;
    
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _textAnimationDuration = 0.7f;
    [SerializeField] private float _buttonOffset = 50f;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private Vector3 _restartButtonInitialPosition;
    private Vector3 _mainMenuButtonInitialPosition;
    private Vector3 _timerTextInitialPosition;
    private Vector3 _meteoriteTextInitialPosition;
    private Vector3 _bombTextInitialPosition;
    private Vector3 _hpTextInitialPosition;

    public event Action RestartClicked;
    public event Action MainMenuClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _restartButtonInitialPosition = _restartButton.transform.position;
        _mainMenuButtonInitialPosition = _mainMenuButton.transform.position;
        _timerTextInitialPosition = _timerText.transform.position;
        _meteoriteTextInitialPosition = _meteoriteText.transform.position;
        _bombTextInitialPosition = _bombText.transform.position;
        _hpTextInitialPosition = _hpText.transform.position;
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameWon += OpenScreen;
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameWon -= OpenScreen;
    }

    private void ProcessMainMenuClicked()
    {
        _mainMenuButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        _screenVisabilityHandler.DisableScreen();
        MainMenuClicked?.Invoke();
    }

    private void ProcessRestartButtonClicked()
    {
        _restartButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        _screenVisabilityHandler.DisableScreen();
        RestartClicked?.Invoke();
    }

    private void OpenScreen()
    {
        UpdateAmounts();
        _screenVisabilityHandler.EnableScreen();
        _winSound.Play();
        PlayEntranceAnimation();
    }

    private void UpdateAmounts()
    {
        _timerText.text = _speedTapGameContoller.GetTimerText();
        _meteoriteText.text = _speedTapGameContoller.MeteoriteCount.ToString();
        _bombText.text = _speedTapGameContoller.BombCount.ToString();
        _hpText.text = _speedTapGameContoller.Health + " hp";
    }

    private void PlayEntranceAnimation()
    {
        _timerText.transform.position = _timerTextInitialPosition + Vector3.down * _buttonOffset;
        _meteoriteText.transform.position = _meteoriteTextInitialPosition + Vector3.down * _buttonOffset;
        _bombText.transform.position = _bombTextInitialPosition + Vector3.down * _buttonOffset;
        _hpText.transform.position = _hpTextInitialPosition + Vector3.down * _buttonOffset;
        _restartButton.transform.position = _restartButtonInitialPosition + Vector3.left * _buttonOffset;
        _mainMenuButton.transform.position = _mainMenuButtonInitialPosition + Vector3.right * _buttonOffset;

        _timerText.transform.DOMove(_timerTextInitialPosition, _textAnimationDuration).SetEase(Ease.OutBack);
        _meteoriteText.transform.DOMove(_meteoriteTextInitialPosition, _textAnimationDuration).SetEase(Ease.OutBack).SetDelay(_textAnimationDuration * 0.2f);
        _bombText.transform.DOMove(_bombTextInitialPosition, _textAnimationDuration).SetEase(Ease.OutBack).SetDelay(_textAnimationDuration * 0.4f);
        _hpText.transform.DOMove(_hpTextInitialPosition, _textAnimationDuration).SetEase(Ease.OutBack).SetDelay(_textAnimationDuration * 0.6f);

        _restartButton.transform.DOMove(_restartButtonInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_textAnimationDuration + _animationDuration * 0.2f);
        _mainMenuButton.transform.DOMove(_mainMenuButtonInitialPosition, _animationDuration).SetEase(Ease.OutBack).SetDelay(_textAnimationDuration + _animationDuration * 0.4f);
    }
}