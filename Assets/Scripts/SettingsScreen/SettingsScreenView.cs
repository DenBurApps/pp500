using System;
using UnityEngine;
using UnityEngine.UI;

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
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    public event Action MusicToggled;
    public event Action SoundToggled;
    public event Action FeedbackClicked;
    public event Action TermsOfUseClicked;
    public event Action PrivacyClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
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
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void ToggleOnMusicSprite()
    {
        _musicButtonImage.sprite = _toggleOnSprite;
    }

    public void ToggleOnSoundSprite()
    {
        _soundButtonImage.sprite = _toggleOnSprite;
    }

    public void ToggleOffSoundSprite()
    {
        _soundButtonImage.sprite = _toggleOffSprite;
    }

    public void ToggleOffMusicSprite()
    {
        _musicButtonImage.sprite = _toggleOffSprite;
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
        BackButtonClicked?.Invoke();
    }
}
