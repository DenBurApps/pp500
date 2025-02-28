using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _speedTapButton;
    [SerializeField] private Button _memoryFlipButton;
    [SerializeField] private Button _shapeSorterButton;

    [SerializeField] private GameObject[] _menuItems;
    
    [Header("Animation Settings")]
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _scaleAnimDuration = 0.5f;
    [SerializeField] private float _staggerDelay = 0.1f;
    [SerializeField] private Ease _fadeInEase = Ease.OutQuad;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;
    [SerializeField] private float _startScale = 0.3f;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private CanvasGroup[] _menuItemCanvasGroups;
    private Sequence _currentAnimation;
    
    public event Action SettingClicked;
    public event Action SpeedTapClicked;
    public event Action MemoryFlipClicked;
    public event Action ShapeSorterClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        _menuItemCanvasGroups = new CanvasGroup[_menuItems.Length];
        
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItemCanvasGroups[i] = _menuItems[i].GetComponent<CanvasGroup>();
            if (_menuItemCanvasGroups[i] == null)
            {
                _menuItemCanvasGroups[i] = _menuItems[i].AddComponent<CanvasGroup>();
            }
            
            _menuItemCanvasGroups[i].alpha = 0f;
            
            RectTransform rect = _menuItems[i].GetComponent<RectTransform>();
            rect.localScale = new Vector3(_startScale, _startScale, _startScale);
        }
    }

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(ProcessSettingsClicked);
        _speedTapButton.onClick.AddListener(ProcessSpeedTapClicked);
        _memoryFlipButton.onClick.AddListener(ProcessMemoryFlipClicked);
        _shapeSorterButton.onClick.AddListener(ProcessShapeSorterClicked);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(ProcessSettingsClicked);
        _speedTapButton.onClick.RemoveListener(ProcessSpeedTapClicked);
        _memoryFlipButton.onClick.RemoveListener(ProcessMemoryFlipClicked);
        _shapeSorterButton.onClick.RemoveListener(ProcessShapeSorterClicked);
        
        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }
    }
    
    public void EnableWithAnimation(float duration, float staggerDelay)
    {
        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
        }
        
        _screenVisabilityHandler.EnableScreen();
        
        _currentAnimation = DOTween.Sequence();
        
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItemCanvasGroups[i].alpha = 0f;
            RectTransform rect = _menuItems[i].GetComponent<RectTransform>();
            rect.localScale = new Vector3(_startScale, _startScale, _startScale);
            
            int index = i;
            
            _currentAnimation.Join(
                DOTween.To(() => 0f, x => _menuItemCanvasGroups[index].alpha = x, 1f, _fadeInDuration)
                    .SetEase(_fadeInEase)
                    .SetDelay(i * staggerDelay)
            );
            
            _currentAnimation.Join(
                rect.DOScale(1f, _scaleAnimDuration)
                    .SetEase(_scaleEase)
                    .SetDelay(i * staggerDelay)
            );
        }
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItemCanvasGroups[i].alpha = 1f;
            RectTransform rect = _menuItems[i].GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
        }
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void DisableWithAnimation(float duration, Action onComplete = null)
    {
        if (_currentAnimation != null)
        {
            _currentAnimation.Kill();
        }
        
        _currentAnimation = DOTween.Sequence();
        
        for (int i = 0; i < _menuItems.Length; i++)
        {
            int index = i;
            
            RectTransform rect = _menuItems[i].GetComponent<RectTransform>();
            _currentAnimation.Join(
                rect.DOScale(_startScale, duration * 0.7f)
                    .SetEase(Ease.InBack)
                    .SetDelay(i * 0.05f)
            );
            
            _currentAnimation.Join(
                DOTween.To(() => 1f, x => _menuItemCanvasGroups[index].alpha = x, 0f, duration)
                    .SetEase(Ease.InQuad)
                    .SetDelay(i * 0.05f)
            );
        }
        
        _currentAnimation.OnComplete(() => {
            _screenVisabilityHandler.DisableScreen();
            onComplete?.Invoke();
            _currentAnimation = null;
        });
    }

    private void ProcessShapeSorterClicked()
    {
        ShapeSorterClicked?.Invoke();
    }

    private void ProcessMemoryFlipClicked()
    {
        MemoryFlipClicked?.Invoke();
    }

    private void ProcessSpeedTapClicked()
    {
        SpeedTapClicked?.Invoke();
    }

    private void ProcessSettingsClicked()
    {
        SettingClicked?.Invoke();
    }
}