using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class LoseScreenView : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _meteoriteText;
    [SerializeField] private TMP_Text _bombText;
    [SerializeField] private AudioSource _loseSound;
    [SerializeField] private SpeedTapGameContoller _speedTapGameContoller;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _animationEase = Ease.OutBack;
    [SerializeField] private float _buttonHoverScale = 1.1f;
    [SerializeField] private CanvasGroup _backgroundPanel;
    [SerializeField] private Transform _loseContainer;
    [SerializeField] private TMP_Text _gameOverText;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _tryAgainOriginalScale;
    private Vector3 _mainMenuOriginalScale;
    private Vector3 _containerOriginalScale;
    private Sequence _animationSequence;

    public event Action TryAgainClicked;
    public event Action MainMenuClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _tryAgainOriginalScale = _tryAgainButton.transform.localScale;
        _mainMenuOriginalScale = _mainMenuButton.transform.localScale;
        
        if (_loseContainer != null)
        {
            _containerOriginalScale = _loseContainer.localScale;
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
        _tryAgainButton.onClick.AddListener(ProcessTryAgainClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameLost += OpenScreen;
        
        AddButtonHoverEffects();
    }

    private void OnDisable()
    {
        _tryAgainButton.onClick.RemoveListener(ProcessTryAgainClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        _speedTapGameContoller.GameLost -= OpenScreen;
        
        RemoveButtonHoverEffects();
        
        _animationSequence?.Kill();
    }

    private void OpenScreen()
    {
        SetValues();
        _screenVisabilityHandler.EnableScreen();
        
        AnimateLoseScreen();
    }

    private void SetValues()
    {
        _timerText.text = _speedTapGameContoller.GetTimerText();
        _meteoriteText.text = _speedTapGameContoller.MeteoriteCount.ToString();
        _bombText.text = _speedTapGameContoller.BombCount.ToString();
    }
    
    private void AnimateLoseScreen()
    {
        _animationSequence?.Kill();
        
        if (_loseContainer != null)
        {
            _loseContainer.localScale = Vector3.zero;
        }
        
        _tryAgainButton.transform.localScale = Vector3.zero;
        _mainMenuButton.transform.localScale = Vector3.zero;
        
        if (_timerText != null)
        {
            _timerText.alpha = 0;
        }
        
        if (_meteoriteText != null)
        {
            _meteoriteText.alpha = 0;
        }
        
        if (_bombText != null)
        {
            _bombText.alpha = 0;
        }
        
        if (_gameOverText != null)
        {
            _gameOverText.transform.localScale = Vector3.zero;
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
        
        if (_loseContainer != null)
        {
            _animationSequence.Append(_loseContainer.DOScale(_containerOriginalScale, _animationDuration).SetEase(_animationEase));
        }
        
        if (_gameOverText != null)
        {
            _animationSequence.Append(_gameOverText.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBounce));
        }
        
        if (_timerText != null)
        {
            _animationSequence.Append(_timerText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _animationSequence.Join(_timerText.transform.DOPunchScale(Vector3.one * 0.3f, _animationDuration, 2, 0.5f));
        }
        
        if (_meteoriteText != null)
        {
            _animationSequence.Append(_meteoriteText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _animationSequence.Join(_meteoriteText.transform.DOPunchScale(Vector3.one * 0.3f, _animationDuration, 2, 0.5f));
        }
        
        if (_bombText != null)
        {
            _animationSequence.Append(_bombText.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
            _animationSequence.Join(_bombText.transform.DOPunchScale(Vector3.one * 0.3f, _animationDuration, 2, 0.5f));
        }
        
        _animationSequence.Append(_tryAgainButton.transform.DOScale(_tryAgainOriginalScale, _animationDuration).SetEase(_animationEase));
        _animationSequence.Append(_mainMenuButton.transform.DOScale(_mainMenuOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.OnStart(() => {
            if (_loseSound != null)
            {
                _loseSound.Play();
            }
        });
        
        _animationSequence.Play();
    }
    
    private void AnimateScreenOut(Action onComplete = null)
    {
        _animationSequence?.Kill();
        
        _animationSequence = DOTween.Sequence();
        
        _animationSequence.Append(_tryAgainButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_mainMenuButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        
        if (_timerText != null)
        {
            _animationSequence.Join(_timerText.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        if (_meteoriteText != null)
        {
            _animationSequence.Join(_meteoriteText.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        if (_bombText != null)
        {
            _animationSequence.Join(_bombText.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        if (_gameOverText != null)
        {
            _animationSequence.Join(_gameOverText.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        }
        
        if (_loseContainer != null)
        {
            _animationSequence.Append(_loseContainer.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
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
        AddButtonHoverEffect(_tryAgainButton, _tryAgainOriginalScale);
        AddButtonHoverEffect(_mainMenuButton, _mainMenuOriginalScale);
    }
    
    private void RemoveButtonHoverEffects()
    {
        RemoveButtonHoverEffect(_tryAgainButton);
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

    private void ProcessTryAgainClicked()
    {
        _tryAgainButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                TryAgainClicked?.Invoke();
                AnimateScreenOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }
}