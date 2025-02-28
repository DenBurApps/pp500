using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SpeedTapDescriptionView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private TMP_Text _destroyedMeteoritesText;
    [SerializeField] private TMP_Text _longestLevelText;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _fadeInEase = Ease.OutQuad;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;
    [SerializeField] private float _startScale = 0.7f;
    [SerializeField] private float _textAnimationDuration = 1.0f;
    [SerializeField] private Ease _textAnimationEase = Ease.OutQuad;
    [SerializeField] private float _textAnimationDelay = 0.3f;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Sequence _currentAnimation;
    private Sequence _textAnimationSequence;
    private int _totalMeteoritesDestroyed;
    private string _maxSurvivalTimeText;

    public event Action BackButtonClicked;

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
        
        _totalMeteoritesDestroyed = PlayerPrefs.GetInt("TotalMeteoritesDestroyed", 0);
        _maxSurvivalTimeText = PlayerPrefs.GetString("SpeedTapMaxSurvivalTimeText", "00:00");
        
        _destroyedMeteoritesText.text = "0";
        _longestLevelText.text = "00:00";
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _mainScreen.SpeedTapOpened += EnableWithAnimation;
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _playButton.onClick.AddListener(ProcessPlayButtonClicked);
    }

    private void OnDisable()
    {
        _mainScreen.SpeedTapOpened -= EnableWithAnimation;
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _playButton.onClick.RemoveListener(ProcessPlayButtonClicked);
        
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
            _currentAnimation = null;
        }
        
        if (_textAnimationSequence != null && _textAnimationSequence.IsActive())
        {
            _textAnimationSequence.Kill();
            _textAnimationSequence = null;
        }
    }
    
    private void EnableWithAnimation()
    {
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
        }
        
        if (_textAnimationSequence != null && _textAnimationSequence.IsActive())
        {
            _textAnimationSequence.Kill();
        }
        
        _destroyedMeteoritesText.text = "0";
        _longestLevelText.text = "00:00";
        
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = new Vector3(_startScale, _startScale, _startScale);
        
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
        
        _currentAnimation.OnComplete(AnimateTextValues);
    }
    
    private void AnimateTextValues()
    {
        _textAnimationSequence = DOTween.Sequence();
        
        if (_totalMeteoritesDestroyed > 0)
        {
            _textAnimationSequence.Append(
                DOTween.To(
                    () => 0,
                    value => _destroyedMeteoritesText.text = Mathf.FloorToInt(value).ToString(),
                    _totalMeteoritesDestroyed,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase)
            );
        }
        
        if (_maxSurvivalTimeText != "00:00")
        {
            string[] timeParts = _maxSurvivalTimeText.Split(':');
            if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int minutes) && int.TryParse(timeParts[1], out int seconds))
            {
                int totalSeconds = minutes * 60 + seconds;
                
                _textAnimationSequence.Join(
                    DOTween.To(
                        () => 0,
                        value => {
                            int currentSeconds = Mathf.FloorToInt(value);
                            int mins = currentSeconds / 60;
                            int secs = currentSeconds % 60;
                            _longestLevelText.text = $"{mins:00}:{secs:00}";
                        },
                        totalSeconds,
                        _textAnimationDuration
                    ).SetEase(_textAnimationEase).SetDelay(_textAnimationDelay)
                );
            }
            else
            {
                _textAnimationSequence.InsertCallback(_textAnimationDelay, () => {
                    _longestLevelText.text = _maxSurvivalTimeText;
                });
            }
        }
    }

    private void DisableWithAnimation(Action onComplete)
    {
        if (_currentAnimation != null && _currentAnimation.IsActive())
        {
            _currentAnimation.Kill();
        }
        
        if (_textAnimationSequence != null && _textAnimationSequence.IsActive())
        {
            _textAnimationSequence.Kill();
            _textAnimationSequence = null;
        }
        
        _currentAnimation = DOTween.Sequence();
        
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
    
    private void ProcessPlayButtonClicked()
    {
        DisableWithAnimation(() => {
            SceneManager.LoadScene("SpeedTapScene");
        });
    }

    private void ProcessBackButtonClicked()
    {
        DisableWithAnimation(() => {
            BackButtonClicked?.Invoke();
        });
    }
}