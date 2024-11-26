using System;
using UnityEngine;
using UnityEngine.UI;

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

    public void DisableWindow(ScreenVisabilityHandler window)
    {
        window.DisableScreen();
    }

    public void EnableWindow(ScreenVisabilityHandler window)
    {
        window.EnableScreen();
    }

    private void ProcessFirstButtonClicked()
    {
        FirstButtonClicked?.Invoke();
    }

    private void ProcessSecondButtonClicked()
    {
        SecondButtonClicked?.Invoke();
    }

    private void ProcessThirdButtonClicked()
    {
        ThridButtonClicked?.Invoke();
    }
}
