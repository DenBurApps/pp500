using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SettingsScreenView : MonoBehaviour
{
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private Button _privacyPolicyButton;
    [SerializeField] private Button _termsOfUseButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Image _musicButtonImage;
    [SerializeField] private Image _soundButtonImage;

    [SerializeField] private Sprite _toggleOffSprite;
    [SerializeField] private Sprite _toggleOnSprite;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _fadeInEase = Ease.OutQuad;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;
    [SerializeField] private float _startScale = 0.7f;
    [SerializeField] private float _buttonAnimationDelay = 0.05f;
    [SerializeField] private RectTransform[] _animatedElements;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Sequence _currentAnimation;

    public event Action BackButtonClicked;
    public event Action MusicToggled;
    public event Action SoundToggled;
    public event Action FeedbackClicked;
    public event Action TermsOfUseClicked;
    public event Action PrivacyClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        _rectTransform = GetComponent<RectTransform>();
        
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = new Vector3(_startScale, _startScale, _startScale);
        
        if (_animatedElements != null && _animatedElements.Length > 0)
        {
            foreach (var element in _animatedElements)
            {
                element.localScale = Vector3.zero;
            }
        }
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(ProcessToggleSoundClicked);
        _musicButton.onClick.AddListener(ProcessToggleMusicClicked);
        _feedbackButton.onClick.AddListener(ProcessFeedbackClicked);
        _privacyPolicyButton.onClick.AddListener(ProcessPrivacyPolicyClicked);
        _termsOfUseButton.onClick.AddListener(ProcessTermsOfUseClicked);
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(ProcessToggleSoundClicked);
        _musicButton.onClick.RemoveListener(ProcessToggleMusicClicked);
        _feedbackButton.onClick.RemoveListener(ProcessFeedbackClicked);
        _privacyPolicyButton.onClick.RemoveListener(ProcessPrivacyPolicyClicked);
        _termsOfUseButton.onClick.RemoveListener(ProcessTermsOfUseClicked);
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        
        _canvasGroup.alpha = 1f;
        _rectTransform.localScale = Vector3.one;
        
        if (_animatedElements != null && _animatedElements.Length > 0)
        {
            foreach (var element in _animatedElements)
            {
                element.localScale = Vector3.one;
            }
        }
    }
    
    public void EnableWithAnimation()
    {
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
        }
        
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = new Vector3(_startScale, _startScale, _startScale);
        
        if (_animatedElements != null && _animatedElements.Length > 0)
        {
            foreach (var element in _animatedElements)
            {
                element.localScale = Vector3.zero;
            }
        }
        
        _screenVisabilityHandler.EnableScreen();
        
        _currentAnimation = DOTween.Sequence();
        
        _currentAnimation.Append(
            DOTween.To(() => 0f, x => _canvasGroup.alpha = x, 1f, _animationDuration)
                .SetEase(_fadeInEase)
        );
        
        _currentAnimation.Join(
            _rectTransform.DOScale(1f, _animationDuration)
                .SetEase(_scaleEase)
        );
        
        if (_animatedElements != null && _animatedElements.Length > 0)
        {
            for (int i = 0; i < _animatedElements.Length; i++)
            {
                int index = i;
                _currentAnimation.Insert(_animationDuration * 0.5f + (i * _buttonAnimationDelay),
                    _animatedElements[index].DOScale(1f, _animationDuration * 0.7f)
                        .SetEase(Ease.OutBack)
                );
            }
        }
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void DisableWithAnimation(Action onComplete = null)
    {
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
        }
        
        _currentAnimation = DOTween.Sequence();
        
        if (_animatedElements != null && _animatedElements.Length > 0)
        {
            for (int i = _animatedElements.Length - 1; i >= 0; i--)
            {
                int index = i;
                _currentAnimation.Insert(0,
                    _animatedElements[index].DOScale(0f, _animationDuration * 0.5f)
                        .SetEase(Ease.InBack)
                        .SetDelay((_animatedElements.Length - 1 - i) * _buttonAnimationDelay * 0.5f)
                );
            }
        }
        
        _currentAnimation.Append(
            _rectTransform.DOScale(_startScale, _animationDuration * 0.8f)
                .SetEase(Ease.InBack)
        );
        
        _currentAnimation.Join(
            DOTween.To(() => 1f, x => _canvasGroup.alpha = x, 0f, _animationDuration)
                .SetEase(Ease.InQuad)
        );
        
        _currentAnimation.OnComplete(() => {
            _screenVisabilityHandler.DisableScreen();
            onComplete?.Invoke();
            _currentAnimation = null;
        });
    }

    public void ToggleOnMusicSprite()
    {
        _musicButtonImage.transform.DOScale(0.8f, 0.1f).OnComplete(() => {
            _musicButtonImage.sprite = _toggleOnSprite;
            _musicButtonImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public void ToggleOnSoundSprite()
    {
        _soundButtonImage.transform.DOScale(0.8f, 0.1f).OnComplete(() => {
            _soundButtonImage.sprite = _toggleOnSprite;
            _soundButtonImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public void ToggleOffSoundSprite()
    {
        _soundButtonImage.transform.DOScale(0.8f, 0.1f).OnComplete(() => {
            _soundButtonImage.sprite = _toggleOffSprite;
            _soundButtonImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public void ToggleOffMusicSprite()
    {
        _musicButtonImage.transform.DOScale(0.8f, 0.1f).OnComplete(() => {
            _musicButtonImage.sprite = _toggleOffSprite;
            _musicButtonImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        });
    }

    private void ProcessToggleSoundClicked()
    {
        SoundToggled?.Invoke();
    }

    private void ProcessToggleMusicClicked()
    {
        MusicToggled?.Invoke();
    }

    private void ProcessFeedbackClicked()
    {
        FeedbackClicked?.Invoke();
    }

    private void ProcessPrivacyPolicyClicked()
    {
        PrivacyClicked?.Invoke();
    }

    private void ProcessTermsOfUseClicked()
    {
        TermsOfUseClicked?.Invoke();
    }

    private void ProcessBackButtonClicked()
    {
        DisableWithAnimation(() => {
            BackButtonClicked?.Invoke();
        });
    }
}