using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterVictoryScreen : MonoBehaviour
{
    [SerializeField] private ShapeSorterGameController _gameController;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private AudioSource _victorySound;
    [SerializeField] private RectTransform _victoryPanel;
    [SerializeField] private float _animationDuration = 0.7f;
    [SerializeField] private GameObject _confettiParticles;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Sequence _celebrationSequence;

    public event Action RestartClicked;
    public event Action MainMenuClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
        
        if (_victoryPanel != null)
        {
            _victoryPanel.localScale = Vector3.zero;
        }
        
        if (_confettiParticles != null)
        {
            _confettiParticles.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _gameController.GameWon += OpenScreen;
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _gameController.GameWon -= OpenScreen;
        
        if (_celebrationSequence != null)
        {
            _celebrationSequence.Kill();
        }
    }

    private void ProcessMainMenuClicked()
    {
        AnimateButtonClick(_mainMenuButton);
        PlayExitAnimation(() => 
        {
            _screenVisabilityHandler.DisableScreen();
            MainMenuClicked?.Invoke();
        });
    }

    private void ProcessRestartButtonClicked()
    {
        AnimateButtonClick(_restartButton);
        PlayExitAnimation(() => 
        {
            _screenVisabilityHandler.DisableScreen();
            RestartClicked?.Invoke();
        });
    }

    private void OpenScreen()
    {
        UpdateAmounts();
        _screenVisabilityHandler.EnableScreen();
        
        if (_confettiParticles != null)
        {
            _confettiParticles.SetActive(true);
        }
        
        PlayVictoryAnimation();
    }

    private void UpdateAmounts()
    {
        _timerText.text = _gameController.View.TimerText;
        _coinsText.text = _gameController.View.CoinsAmount.text;
        
        _timerText.color = new Color(_timerText.color.r, _timerText.color.g, _timerText.color.b, 0);
        _coinsText.color = new Color(_coinsText.color.r, _coinsText.color.g, _coinsText.color.b, 0);
    }
    
    private void AnimateButtonClick(Button button)
    {
        button.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
    }
    
    private void PlayVictoryAnimation()
    {
        _victorySound.Play();
        
        if (_victoryPanel != null)
        {
            _celebrationSequence = DOTween.Sequence();
            
            _celebrationSequence.Append(_victoryPanel.DOScale(1.2f, _animationDuration).SetEase(Ease.OutBack));
            _celebrationSequence.Append(_victoryPanel.DOScale(1f, _animationDuration / 2).SetEase(Ease.OutBack));
            
            _celebrationSequence.Append(_timerText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _celebrationSequence.Join(_coinsText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            
            _restartButton.transform.localScale = Vector3.zero;
            _mainMenuButton.transform.localScale = Vector3.zero;
            
            _celebrationSequence.Append(_restartButton.transform.DOScale(1, _animationDuration / 2).SetEase(Ease.OutBack));
            _celebrationSequence.Append(_mainMenuButton.transform.DOScale(1, _animationDuration / 2).SetEase(Ease.OutBack));
            
            _celebrationSequence.Play();
        }
    }
    
    private void PlayExitAnimation(Action onComplete)
    {
        Sequence exitSequence = DOTween.Sequence();
        
        if (_victoryPanel != null)
        {
            exitSequence.Append(_victoryPanel.DOScale(0, _animationDuration).SetEase(Ease.InBack));
        }
        
        if (_confettiParticles != null)
        {
            exitSequence.AppendCallback(() => _confettiParticles.SetActive(false));
        }
        
        exitSequence.AppendCallback(() => onComplete?.Invoke());
        exitSequence.Play();
    }
}