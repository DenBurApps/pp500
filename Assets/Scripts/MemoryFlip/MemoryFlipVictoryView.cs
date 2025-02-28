using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipVictoryView : MonoBehaviour
{
    [SerializeField] private MemeoryFlipGameController _gameController;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private AudioSource _victorySound;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _animationEase = Ease.OutBack;
    [SerializeField] private float _buttonHoverScale = 1.1f;
    [SerializeField] private float _textAnimationDelay = 0.2f;
    [SerializeField] private CanvasGroup _backgroundPanel;
    [SerializeField] private Transform _victoryContainer;
    [SerializeField] private TMP_Text _congratulationsText;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _restartOriginalScale;
    private Vector3 _mainMenuOriginalScale;
    private Vector3 _containerOriginalScale;
    private Sequence _animationSequence;

    public event Action RestartClicked;
    public event Action MainMenuClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _restartOriginalScale = _restartButton.transform.localScale;
        _mainMenuOriginalScale = _mainMenuButton.transform.localScale;
        
        if (_victoryContainer != null)
        {
            _containerOriginalScale = _victoryContainer.localScale;
        }
        
        if (_backgroundPanel == null)
        {
            _backgroundPanel = GetComponentInChildren<CanvasGroup>();
        }
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _gameController.GameWon += OpenScreen;
        
        AddButtonHoverEffects();
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(ProcessRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _gameController.GameWon -= OpenScreen;
        
        RemoveButtonHoverEffects();
        
        _animationSequence?.Kill();
    }

    private void OpenScreen()
    {
        UpdateAmounts();
        _screenVisabilityHandler.EnableScreen();
        SaveVictoryCount();
        
        AnimateVictoryScreen();
    }

    private void UpdateAmounts()
    {
        _timerText.text = _gameController.View.TimerText;
        _levelText.text = _gameController.View.DifficultyText;
    }
    
    private void SaveVictoryCount()
    {
        if (PlayerPrefs.HasKey("MemoryFlipWinCount"))
        {
            int savedLoseCount = PlayerPrefs.GetInt("MemoryFlipWinCount");
            PlayerPrefs.SetInt("MemoryFlipWinCount", savedLoseCount + 1);
        }
        else
        {
            PlayerPrefs.SetInt("MemoryFlipWinCount", 1);
        }
        
        PlayerPrefs.Save();
    }
    
    private void AnimateVictoryScreen()
    {
        _animationSequence?.Kill();
        
        if (_victoryContainer != null)
        {
            _victoryContainer.localScale = Vector3.zero;
        }
        
        _restartButton.transform.localScale = Vector3.zero;
        _mainMenuButton.transform.localScale = Vector3.zero;
        
        if (_timerText != null)
        {
            _timerText.alpha = 0;
        }
        
        if (_levelText != null)
        {
            _levelText.alpha = 0;
        }
        
        if (_congratulationsText != null)
        {
            _congratulationsText.transform.localScale = Vector3.zero;
        }
        
        if (_backgroundPanel != null)
        {
            _backgroundPanel.alpha = 0;
        }
        
        _animationSequence = DOTween.Sequence();
        
        if (_backgroundPanel != null)
        {
            _animationSequence.Append(_backgroundPanel.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
        }
        if (_victoryContainer != null)
        {
            _animationSequence.Append(_victoryContainer.DOScale(_containerOriginalScale, _animationDuration).SetEase(_animationEase));
        }
        
        if (_congratulationsText != null)
        {
            _animationSequence.Append(_congratulationsText.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBounce));
        }
        
        if (_timerText != null)
        {
            _animationSequence.Append(_timerText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _animationSequence.Join(_timerText.transform.DOPunchScale(Vector3.one * 0.3f, _animationDuration, 2, 0.5f));
        }
        
        if (_levelText != null)
        {
            _animationSequence.Append(_levelText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _animationSequence.Join(_levelText.transform.DOPunchScale(Vector3.one * 0.3f, _animationDuration, 2, 0.5f));
        }
        
        _animationSequence.Append(_restartButton.transform.DOScale(_restartOriginalScale, _animationDuration).SetEase(_animationEase));
        _animationSequence.Append(_mainMenuButton.transform.DOScale(_mainMenuOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.OnStart(() => {
            if (_victorySound != null)
            {
                _victorySound.Play();
            }
        });
        
        _animationSequence.Play();
    }
    
    private void AnimateScreenOut(Action onComplete = null)
    {
        _animationSequence?.Kill();
        
        _animationSequence = DOTween.Sequence();
        
        _animationSequence.Append(_restartButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_mainMenuButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        
        if (_timerText != null)
        {
            _animationSequence.Join(_timerText.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        if (_levelText != null)
        {
            _animationSequence.Join(_levelText.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        if (_congratulationsText != null)
        {
            _animationSequence.Join(_congratulationsText.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        }
        
        if (_victoryContainer != null)
        {
            _animationSequence.Append(_victoryContainer.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        }
        
        if (_backgroundPanel != null)
        {
            _animationSequence.Append(_backgroundPanel.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        _animationSequence.OnComplete(() => onComplete?.Invoke());
        
        _animationSequence.Play();
    }
    
    private void AddButtonHoverEffects()
    {
        AddButtonHoverEffect(_restartButton, _restartOriginalScale);
        AddButtonHoverEffect(_mainMenuButton, _mainMenuOriginalScale);
    }
    
    private void RemoveButtonHoverEffects()
    {
        RemoveButtonHoverEffect(_restartButton);
        RemoveButtonHoverEffect(_mainMenuButton);
    }
    
    private void AddButtonHoverEffect(Button button, Vector3 originalScale)
    {
        if (button == null) return;
        
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();
            
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => {
            button.transform.DOScale(originalScale * _buttonHoverScale, 0.2f).SetEase(Ease.OutQuad);
        });
        trigger.triggers.Add(enterEntry);
        
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => {
            button.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
        });
        trigger.triggers.Add(exitEntry);
    }
    
    private void RemoveButtonHoverEffect(Button button)
    {
        if (button == null) return;
        
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            Destroy(trigger);
        }
    }

    private void ProcessMainMenuClicked()
    {
        _mainMenuButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                MainMenuClicked?.Invoke();
                AnimateScreenOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }

    private void ProcessRestartButtonClicked()
    {
        _restartButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                RestartClicked?.Invoke();
                AnimateScreenOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }
}