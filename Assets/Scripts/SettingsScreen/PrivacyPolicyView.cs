using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class PrivacyPolicyView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private SettingsScreen _settings;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(ProcessBackButton);
        _settings.OpenPrivacyPolicy += Enable;
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(ProcessBackButton);
        _settings.OpenPrivacyPolicy -= Enable;
    }

    private void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    private void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessBackButton()
    {
        BackButtonClicked?.Invoke();
        Disable();
    }
}
