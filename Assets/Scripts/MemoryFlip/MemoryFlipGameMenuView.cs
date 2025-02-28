using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipGameMenuView : MonoBehaviour
{
    [SerializeField] private MemeoryFlipGameController _gameController;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _buttonsDelay = 0.1f;
    [SerializeField] private Ease _animationEase = Ease.OutBack;
    [SerializeField] private float _buttonHoverScale = 1.1f;
    [SerializeField] private CanvasGroup _backgroundPanel;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _continueOriginalScale;
    private Vector3 _restartOriginalScale;
    private Vector3 _mainMenuOriginalScale;
    private Sequence _animationSequence;
    
    public event Action MainMenuClicked;
    public event Action RestartButtonClicked;
    public event Action ContinueClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _continueOriginalScale = _continueButton.transform.localScale;
        _restartOriginalScale = _restartButton.transform.localScale;
        _mainMenuOriginalScale = _mainMenuButton.transform.localScale;
        
        if (_backgroundPanel == null)
        {
            _backgroundPanel = GetComponentInChildren<CanvasGroup>();
        }
    }

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        _gameController.OpenMenu += Enable;
        _continueButton.onClick.AddListener(ProcessContinueClicked);
        _restartButton.onClick.AddListener(ProcessRestartClicked);
        _mainMenuButton.onClick.AddListener(ProcessMainMenuClicked);
        
        AddButtonHoverEffects();
    }

    private void OnDisable()
    {
        _gameController.OpenMenu -= Enable;
        _continueButton.onClick.RemoveListener(ProcessContinueClicked);
        _restartButton.onClick.RemoveListener(ProcessRestartClicked);
        _mainMenuButton.onClick.RemoveListener(ProcessMainMenuClicked);
        
        RemoveButtonHoverEffects();
        
        _animationSequence?.Kill();
    }

    private void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        AnimateMenuIn();
    }

    private void Disable()
    {
        AnimateMenuOut(() => _screenVisabilityHandler.DisableScreen());
    }
    
    private void AnimateMenuIn()
    {
        _animationSequence?.Kill();
        
        _continueButton.transform.localScale = Vector3.zero;
        _restartButton.transform.localScale = Vector3.zero;
        _mainMenuButton.transform.localScale = Vector3.zero;
        
        if (_backgroundPanel != null)
        {
            _backgroundPanel.alpha = 0;
        }
        
        _animationSequence = DOTween.Sequence();
        
        if (_backgroundPanel != null)
        {
            _animationSequence.Append(_backgroundPanel.DOFade(1, _animationDuration / 2).SetEase(Ease.OutQuad));
        }
        
        _animationSequence.Append(_continueButton.transform.DOScale(_continueOriginalScale, _animationDuration).SetEase(_animationEase));
        _animationSequence.Append(_restartButton.transform.DOScale(_restartOriginalScale, _animationDuration).SetEase(_animationEase));
        _animationSequence.Append(_mainMenuButton.transform.DOScale(_mainMenuOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.Play();
    }
    
    private void AnimateMenuOut(Action onComplete = null)
    {
        _animationSequence?.Kill();
        
        _animationSequence = DOTween.Sequence();
        
        _animationSequence.Append(_continueButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_restartButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_mainMenuButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        
        if (_backgroundPanel != null)
        {
            _animationSequence.Append(_backgroundPanel.DOFade(0, _animationDuration / 2).SetEase(Ease.InQuad));
        }
        
        _animationSequence.OnComplete(() => onComplete?.Invoke());
        
        _animationSequence.Play();
    }
    
    private void AddButtonHoverEffects()
    {
        AddButtonHoverEffect(_continueButton, _continueOriginalScale);
        AddButtonHoverEffect(_restartButton, _restartOriginalScale);
        AddButtonHoverEffect(_mainMenuButton, _mainMenuOriginalScale);
    }
    
    private void RemoveButtonHoverEffects()
    {
        RemoveButtonHoverEffect(_continueButton);
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
    
    private void ProcessRestartClicked()
    {
        _restartButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                RestartButtonClicked?.Invoke();
                AnimateMenuOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }

    private void ProcessContinueClicked()
    {
        _continueButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                ContinueClicked?.Invoke();
                AnimateMenuOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }
    
    private void ProcessMainMenuClicked()
    {
        _mainMenuButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                MainMenuClicked?.Invoke();
                AnimateMenuOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }
}