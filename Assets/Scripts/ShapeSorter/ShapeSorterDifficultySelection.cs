using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterDifficultySelection : MonoBehaviour
{
    [SerializeField] private ShapeSorterMainMenu _mainMenu;
    [SerializeField] private Button _easyButton;
    [SerializeField] private Button _normalButton;
    [SerializeField] private Button _hardButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private ShapeSorterGameController _gameController;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _buttonSpacing = 0.1f;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Vector3 _easyInitialScale;
    private Vector3 _normalInitialScale;
    private Vector3 _hardInitialScale;
    private Vector3 _backInitialScale;

    public event Action EasySelected;
    public event Action NormalSelected;
    public event Action HardSelected;
    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        _easyInitialScale = _easyButton.transform.localScale;
        _normalInitialScale = _normalButton.transform.localScale;
        _hardInitialScale = _hardButton.transform.localScale;
        _backInitialScale = _backButton.transform.localScale;
    }
    
    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void OnEnable()
    {
        _mainMenu.NewGame += EnableWithAnimation;
        _easyButton.onClick.AddListener(ProcessEasyButtonSelected);
        _normalButton.onClick.AddListener(ProcessNormalButtonSelected);
        _hardButton.onClick.AddListener(ProcessHardButtonSelected);
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _gameController.GameRestarted += EnableWithAnimation;
    }

    private void OnDisable()
    {
        _mainMenu.NewGame -= EnableWithAnimation;
        _easyButton.onClick.RemoveListener(ProcessEasyButtonSelected);
        _normalButton.onClick.RemoveListener(ProcessNormalButtonSelected);
        _hardButton.onClick.RemoveListener(ProcessHardButtonSelected);
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _gameController.GameRestarted -= EnableWithAnimation;
    }
    
    private void EnableWithAnimation()
    {
        _screenVisabilityHandler.EnableScreen();
        PlayEntranceAnimation();
    }
    
    private void ProcessEasyButtonSelected()
    {
        AnimateButtonClick(_easyButton);
        PlayExitAnimation(() => 
        {
            EasySelected?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        });
    }

    private void ProcessNormalButtonSelected()
    {
        AnimateButtonClick(_normalButton);
        PlayExitAnimation(() => 
        {
            NormalSelected?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        });
    }

    private void ProcessHardButtonSelected()
    {
        AnimateButtonClick(_hardButton);
        PlayExitAnimation(() => 
        {
            HardSelected?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        });
    }

    private void ProcessBackButtonClicked()
    {
        AnimateButtonClick(_backButton);
        PlayExitAnimation(() => 
        {
            BackButtonClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        });
    }
    
    private void AnimateButtonClick(Button button)
    {
        button.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), _animationDuration, 1, 0.5f);
    }
    
    private void PlayEntranceAnimation()
    {
        _easyButton.transform.localScale = Vector3.zero;
        _normalButton.transform.localScale = Vector3.zero;
        _hardButton.transform.localScale = Vector3.zero;
        _backButton.transform.localScale = Vector3.zero;
        
        _easyButton.transform.DOScale(_easyInitialScale, _animationDuration).SetEase(Ease.OutBack);
        _normalButton.transform.DOScale(_normalInitialScale, _animationDuration).SetEase(Ease.OutBack).SetDelay(_buttonSpacing);
        _hardButton.transform.DOScale(_hardInitialScale, _animationDuration).SetEase(Ease.OutBack).SetDelay(_buttonSpacing * 2);
        _backButton.transform.DOScale(_backInitialScale, _animationDuration).SetEase(Ease.OutBack).SetDelay(_buttonSpacing * 3);
    }
    
    private void PlayExitAnimation(Action onComplete)
    {
        _easyButton.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack);
        _normalButton.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack).SetDelay(_buttonSpacing);
        _hardButton.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack).SetDelay(_buttonSpacing * 2);
        _backButton.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack).SetDelay(_buttonSpacing * 3).OnComplete(() => onComplete?.Invoke());
    }
}