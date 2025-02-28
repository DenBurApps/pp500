using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Onboarding : MonoBehaviour
{
    [SerializeField] private OnboardingView _view;
    [SerializeField] private ScreenVisabilityHandler _firstWindow;
    [SerializeField] private ScreenVisabilityHandler _secondWindow;
    [SerializeField] private ScreenVisabilityHandler _thirdWindow;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _inEase = Ease.OutBack;
    [SerializeField] private Ease _outEase = Ease.InBack;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Onboarding"))
            SceneManager.LoadScene("MainScene");
    }

    private void Start()
    {
        _view.EnableWindowWithAnimation(_firstWindow, _animationDuration, _inEase);
        _view.DisableWindowImmediate(_secondWindow);
        _view.DisableWindowImmediate(_thirdWindow);
    }

    private void OnEnable()
    {
        _view.FirstButtonClicked += ProcessFirstScreenButtonClicked;
        _view.SecondButtonClicked += ProcessSecondScreenButtonClicked;
        _view.ThridButtonClicked += ProcessThirdScreenButtonClicked;
    }

    private void OnDisable()
    {
        _view.FirstButtonClicked -= ProcessFirstScreenButtonClicked;
        _view.SecondButtonClicked -= ProcessSecondScreenButtonClicked;
        _view.ThridButtonClicked -= ProcessThirdScreenButtonClicked;
    }

    public void ProcessFirstScreenButtonClicked()
    {
        _view.DisableWindowWithAnimation(_firstWindow, _animationDuration / 2, _outEase);
        _view.EnableWindowWithAnimation(_secondWindow, _animationDuration, _inEase);
        _view.DisableWindowImmediate(_thirdWindow);
    }

    private void ProcessSecondScreenButtonClicked()
    {
        _view.DisableWindowWithAnimation(_secondWindow, _animationDuration / 2, _outEase);
        _view.EnableWindowWithAnimation(_thirdWindow, _animationDuration, _inEase);
        _view.DisableWindowImmediate(_firstWindow);
        
        PlayerPrefs.SetInt("Onboarding", 1);
    }

    private void ProcessThirdScreenButtonClicked()
    {
        _view.DisableWindowWithAnimation(_thirdWindow, _animationDuration / 2, _outEase)
            .OnComplete(() => SceneManager.LoadScene("MainScene"));
    }
}
