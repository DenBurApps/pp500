using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterDescriptionView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private TMP_Text _easyLevelPassedText;
    [SerializeField] private TMP_Text _normalLevelPassedText;
    [SerializeField] private TMP_Text _hardLevelPassedText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _coinText;
    
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
    
    private int _easyLevelPassed;
    private int _normalLevelPassed;
    private int _hardLevelPassed;
    private int _totalCoins;
    private float _totalTime;

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
        
        _easyLevelPassed = int.TryParse(PlayerPrefs.GetString("ShapeSorterEasyPassed", "0"), out int easy) ? easy : 0;
        _normalLevelPassed = int.TryParse(PlayerPrefs.GetString("ShapeSorterNormalPassed", "0"), out int normal) ? normal : 0;
        _hardLevelPassed = int.TryParse(PlayerPrefs.GetString("ShapeSorterHardPassed", "0"), out int hard) ? hard : 0;
        _totalCoins = int.TryParse(PlayerPrefs.GetString("ShapeSorterTotalCoins", "0"), out int coins) ? coins : 0;
        _totalTime = PlayerPrefs.GetFloat("ShapeSorterTotalTime", 0f);
        
        _easyLevelPassedText.text = "0";
        _normalLevelPassedText.text = "0";
        _hardLevelPassedText.text = "0";
        _coinText.text = "0";
        _timeText.text = "00:00";
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _mainScreen.ShapeSorterOpened += EnableWithAnimation;
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _playButton.onClick.AddListener(ProcessPlayButtonClicked);
    }

    private void OnDisable()
    {
        _mainScreen.ShapeSorterOpened -= EnableWithAnimation;
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
        
        _easyLevelPassedText.text = "0";
        _normalLevelPassedText.text = "0";
        _hardLevelPassedText.text = "0";
        _coinText.text = "0";
        _timeText.text = "00:00";
        
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
        
        if (_easyLevelPassed > 0)
        {
            _textAnimationSequence.Join(
                DOTween.To(
                    () => 0,
                    value => _easyLevelPassedText.text = Mathf.FloorToInt(value).ToString(),
                    _easyLevelPassed,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase)
            );
        }
        
        if (_normalLevelPassed > 0)
        {
            _textAnimationSequence.Join(
                DOTween.To(
                    () => 0,
                    value => _normalLevelPassedText.text = Mathf.FloorToInt(value).ToString(),
                    _normalLevelPassed,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase).SetDelay(0.1f)
            );
        }
        
        if (_hardLevelPassed > 0)
        {
            _textAnimationSequence.Join(
                DOTween.To(
                    () => 0,
                    value => _hardLevelPassedText.text = Mathf.FloorToInt(value).ToString(),
                    _hardLevelPassed,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase).SetDelay(0.2f)
            );
        }
        
        if (_totalCoins > 0)
        {
            _textAnimationSequence.Join(
                DOTween.To(
                    () => 0,
                    value => _coinText.text = Mathf.FloorToInt(value).ToString(),
                    _totalCoins,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase).SetDelay(0.3f)
            );
        }
        
        if (_totalTime > 0)
        {
            _textAnimationSequence.Join(
                DOTween.To(
                    () => 0f,
                    value => {
                        float currentTime = value;
                        int minutes = Mathf.FloorToInt(currentTime / 60);
                        int seconds = Mathf.FloorToInt(currentTime % 60);
                        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                    },
                    _totalTime + 1,
                    _textAnimationDuration
                ).SetEase(_textAnimationEase).SetDelay(0.4f)
            );
        }
    }
    
    private void DisableWithAnimation(Action onComplete = null)
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

    private void ProcessBackButtonClicked()
    {
        DisableWithAnimation(() => {
            BackButtonClicked?.Invoke();
        });
    }

    private void ProcessPlayButtonClicked()
    {
        DisableWithAnimation(() => {
            SceneManager.LoadScene("ShapeSorterScene");
        });
    }
}