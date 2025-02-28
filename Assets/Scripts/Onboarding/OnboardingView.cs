using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OnboardingView : MonoBehaviour
{
    [SerializeField] private Button _firstScreenButton;
    [SerializeField] private Button _secondScreenButton;
    [SerializeField] private Button _thirdScreenButton;

    public event Action FirstButtonClicked;
    public event Action SecondButtonClicked;
    public event Action ThridButtonClicked;

    private void OnEnable()
    {
        _firstScreenButton.onClick.AddListener(ProcessFirstButtonClicked);
        _secondScreenButton.onClick.AddListener(ProcessSecondButtonClicked);
        _thirdScreenButton.onClick.AddListener(ProcessThirdButtonClicked);
    }

    private void OnDisable()
    {
        _firstScreenButton.onClick.RemoveListener(ProcessFirstButtonClicked);
        _secondScreenButton.onClick.RemoveListener(ProcessSecondButtonClicked);
        _thirdScreenButton.onClick.RemoveListener(ProcessThirdButtonClicked);
    }

    public void DisableWindowImmediate(ScreenVisabilityHandler window)
    {
        window.DisableScreen();
    }

    public void EnableWindowImmediate(ScreenVisabilityHandler window)
    {
        window.EnableScreen();
    }

    public Tween EnableWindowWithAnimation(ScreenVisabilityHandler window, float duration, Ease ease)
    {
        RectTransform rect = window.GetComponent<RectTransform>();

        rect.localScale = Vector3.zero;
        window.EnableScreen();

        return rect.DOScale(Vector3.one, duration)
            .SetEase(ease);
    }

    public Tween DisableWindowWithAnimation(ScreenVisabilityHandler window, float duration, Ease ease)
    {
        RectTransform rect = window.GetComponent<RectTransform>();

        return rect.DOScale(Vector3.zero, duration)
            .SetEase(ease)
            .OnComplete(() => window.DisableScreen());
    }

    private void ProcessFirstButtonClicked()
    {
        AnimateButtonClick(_firstScreenButton);
        FirstButtonClicked?.Invoke();
    }

    private void ProcessSecondButtonClicked()
    {
        AnimateButtonClick(_secondScreenButton);
        SecondButtonClicked?.Invoke();
    }

    private void ProcessThirdButtonClicked()
    {
        AnimateButtonClick(_thirdScreenButton);
        ThridButtonClicked?.Invoke();
    }

    private void AnimateButtonClick(Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 5, 1);
    }
}