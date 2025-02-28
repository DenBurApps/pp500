using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipDificultySelection : MonoBehaviour
{
    [SerializeField] private MemoryFlipMainMenu _mainMenu;
    [SerializeField] private Button _easyButton;
    [SerializeField] private Button _normalButton;
    [SerializeField] private Button _hardButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private MemeoryFlipGameController _gameController;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _buttonsDelay = 0.1f;
    [SerializeField] private Ease _animationEase = Ease.OutBack;
    [SerializeField] private float _buttonHoverScale = 1.1f;
    [SerializeField] private float _initialOffsetY = 100f;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _easyOriginalScale;
    private Vector3 _normalOriginalScale;
    private Vector3 _hardOriginalScale;
    private Vector3 _backOriginalScale;
    private Vector3 _easyOriginalPosition;
    private Vector3 _normalOriginalPosition;
    private Vector3 _hardOriginalPosition;
    private Vector3 _backOriginalPosition;
    private Sequence _animationSequence;

    public event Action EasySelected;
    public event Action NormalSelected;
    public event Action HardSelected;
    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _easyOriginalScale = _easyButton.transform.localScale;
        _normalOriginalScale = _normalButton.transform.localScale;
        _hardOriginalScale = _hardButton.transform.localScale;
        _backOriginalScale = _backButton.transform.localScale;
        
        _easyOriginalPosition = _easyButton.transform.position;
        _normalOriginalPosition = _normalButton.transform.position;
        _hardOriginalPosition = _hardButton.transform.position;
        _backOriginalPosition = _backButton.transform.position;
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _mainMenu.StartGame += OnEnableScreen;
        _easyButton.onClick.AddListener(ProcessEasyButtonSelected);
        _normalButton.onClick.AddListener(ProcessNormalButtonSelected);
        _hardButton.onClick.AddListener(ProcessHardButtonSelected);
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _gameController.GameRestarted += OnEnableScreen;
        
        AddButtonHoverEffects();
    }

    private void OnDisable()
    {
        _mainMenu.StartGame -= OnEnableScreen;
        _easyButton.onClick.RemoveListener(ProcessEasyButtonSelected);
        _normalButton.onClick.RemoveListener(ProcessNormalButtonSelected);
        _hardButton.onClick.RemoveListener(ProcessHardButtonSelected);
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _gameController.GameRestarted -= OnEnableScreen;
        
        RemoveButtonHoverEffects();
        
        _animationSequence?.Kill();
    }
    
    private void OnEnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        AnimateButtonsIn();
    }

    private void AnimateButtonsIn()
    {
        _animationSequence?.Kill();
        
        _easyButton.transform.position = _easyOriginalPosition + new Vector3(0, -_initialOffsetY, 0);
        _normalButton.transform.position = _normalOriginalPosition + new Vector3(0, -_initialOffsetY, 0);
        _hardButton.transform.position = _hardOriginalPosition + new Vector3(0, -_initialOffsetY, 0);
        _backButton.transform.position = _backOriginalPosition + new Vector3(0, -_initialOffsetY, 0);
        
        _easyButton.transform.localScale = Vector3.zero;
        _normalButton.transform.localScale = Vector3.zero;
        _hardButton.transform.localScale = Vector3.zero;
        _backButton.transform.localScale = Vector3.zero;
        
        _animationSequence = DOTween.Sequence();
        
        _animationSequence.Append(_easyButton.transform.DOMove(_easyOriginalPosition, _animationDuration).SetEase(_animationEase));
        _animationSequence.Join(_easyButton.transform.DOScale(_easyOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.Append(_normalButton.transform.DOMove(_normalOriginalPosition, _animationDuration).SetEase(_animationEase));
        _animationSequence.Join(_normalButton.transform.DOScale(_normalOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.Append(_hardButton.transform.DOMove(_hardOriginalPosition, _animationDuration).SetEase(_animationEase));
        _animationSequence.Join(_hardButton.transform.DOScale(_hardOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.Append(_backButton.transform.DOMove(_backOriginalPosition, _animationDuration).SetEase(_animationEase));
        _animationSequence.Join(_backButton.transform.DOScale(_backOriginalScale, _animationDuration).SetEase(_animationEase));
        
        _animationSequence.Play();
    }
    
    private void AnimateButtonsOut(Action onComplete = null)
    {
        _animationSequence?.Kill();
        
        _animationSequence = DOTween.Sequence();
        
        _animationSequence.Append(_easyButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_normalButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_hardButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        _animationSequence.Join(_backButton.transform.DOScale(Vector3.zero, _animationDuration / 2).SetEase(Ease.InBack));
        
        _animationSequence.OnComplete(() => onComplete?.Invoke());
        
        _animationSequence.Play();
    }
    
    private void AddButtonHoverEffects()
    {
        AddButtonHoverEffect(_easyButton, _easyOriginalScale);
        AddButtonHoverEffect(_normalButton, _normalOriginalScale);
        AddButtonHoverEffect(_hardButton, _hardOriginalScale);
        AddButtonHoverEffect(_backButton, _backOriginalScale);
    }
    
    private void RemoveButtonHoverEffects()
    {
        RemoveButtonHoverEffect(_easyButton);
        RemoveButtonHoverEffect(_normalButton);
        RemoveButtonHoverEffect(_hardButton);
        RemoveButtonHoverEffect(_backButton);
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

    private void ProcessEasyButtonSelected()
    {
        _easyButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                EasySelected?.Invoke();
                AnimateButtonsOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }

    private void ProcessNormalButtonSelected()
    {
        _normalButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                NormalSelected?.Invoke();
                AnimateButtonsOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }

    private void ProcessHardButtonSelected()
    {
        _hardButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                HardSelected?.Invoke();
                AnimateButtonsOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }

    private void ProcessBackButtonClicked()
    {
        _backButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .OnComplete(() => {
                BackButtonClicked?.Invoke();
                AnimateButtonsOut(() => _screenVisabilityHandler.DisableScreen());
            });
    }
}