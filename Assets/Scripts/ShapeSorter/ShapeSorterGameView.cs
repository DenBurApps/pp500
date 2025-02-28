using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterGameView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _coinsAmount;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _squareQuantity;
    [SerializeField] private TMP_Text _triangleQuantity;
    [SerializeField] private TMP_Text _circleQuantity;
    [SerializeField] private TMP_Text _rectangleQuantity;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _popupScale = 1.2f;
    [SerializeField] private float _fadeInDelay = 0.1f;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private CanvasGroup _canvasGroup;

    public event Action MenuButtonClicked;

    public TMP_Text CoinsAmount => _coinsAmount;
    public TMP_Text SquareQuantity => _squareQuantity;
    public TMP_Text TriangleQuantity => _triangleQuantity;
    public TMP_Text CircleQuantity => _circleQuantity;
    public TMP_Text RectangleQuantity => _rectangleQuantity;
    public string TimerText => _timer.text;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnEnable()
    {
        _menuButton.onClick.AddListener(ProcessMenuClicked);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(ProcessMenuClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        PlayEntranceAnimation();
    }

    public void Disable()
    {
        PlayExitAnimation(() => _screenVisabilityHandler.DisableScreen());
    }
    
    public void SetTimerValue(float minutes, float seconds)
    {
        string formattedTime = string.Format("{0:00} : {1:00}", minutes, seconds);
        if (_timer.text != formattedTime)
        {
            _timer.text = formattedTime;
            _timer.transform.DOPunchScale(Vector3.one * 0.1f, _animationDuration / 2, 1, 0.5f);
        }
    }

    public void SetTextAmount(int amount, TMP_Text textToSet)
    {
        if (textToSet.text != amount.ToString())
        {
            int oldAmount = 0;
            int.TryParse(textToSet.text, out oldAmount);
            
            textToSet.text = amount.ToString();
            
            if (amount > oldAmount)
            {
                textToSet.transform.DOScale(_popupScale, _animationDuration / 2).OnComplete(() => {
                    textToSet.transform.DOScale(1f, _animationDuration / 2);
                });
                textToSet.DOColor(Color.yellow, _animationDuration / 2).OnComplete(() => {
                    textToSet.DOColor(Color.white, _animationDuration / 2);
                });
            }
        }
    }

    private void ProcessMenuClicked()
    {
        _menuButton.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration / 2, 1, 0.5f);
        MenuButtonClicked?.Invoke();
    }
    
    private void PlayEntranceAnimation()
    {
        TMP_Text[] uiElements = { _coinsAmount, _timer, _squareQuantity, _triangleQuantity, _circleQuantity, _rectangleQuantity };
        
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, _animationDuration).SetEase(Ease.OutQuad);
        
        foreach (var element in uiElements)
        {
            element.transform.localScale = Vector3.zero;
        }
        
        _menuButton.transform.localScale = Vector3.zero;
        
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].transform.DOScale(1, _animationDuration)
                            .SetEase(Ease.OutBack)
                            .SetDelay(_fadeInDelay * i);
        }
        
        _menuButton.transform.DOScale(1, _animationDuration)
                    .SetEase(Ease.OutBack)
                    .SetDelay(_fadeInDelay * uiElements.Length);
    }
    
    private void PlayExitAnimation(Action onComplete)
    {
        _canvasGroup.DOFade(0, _animationDuration)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => onComplete?.Invoke());
    }
}