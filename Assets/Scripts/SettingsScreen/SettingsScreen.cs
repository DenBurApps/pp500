using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private SettingsScreenView _view;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private PrivacyPolicyView _privacyPolicy;
    [SerializeField] private TermsOfUseView _termsOfUse;
    [SerializeField] private AudioMixerGroup _audioMixer;

    private bool _musicOn = true;
    private bool _soundOn = true;

    public event Action OpenTermsOfUse;
    public event Action OpenPrivacyPolicy;
    public event Action BackButtonClicked;

    private void Start()
    {
        _view.Disable();

        _musicOn = PlayerPrefs.GetInt("MusicOff", 0) == 0;
        _soundOn = PlayerPrefs.GetInt("SoundOff", 0) == 0;
        
        
        if (_musicOn)
        {
            _view.ToggleOnMusicSprite();
            _audioMixer.audioMixer.SetFloat("Music", -20f);
        }
        else
        {
            _view.ToggleOffMusicSprite();
            _audioMixer.audioMixer.SetFloat("Music", -80f);
        }
        
        if (_soundOn)
        {
            _view.ToggleOnSoundSprite();
            _audioMixer.audioMixer.SetFloat("Effects", -20f);
        }
        else
        {
            _view.ToggleOffSoundSprite();
            _audioMixer.audioMixer.SetFloat("Effects", -80f);
        }
    }

    private void OnEnable()
    {
        _mainScreen.SettingsOpened += _view.Enable;
        _view.SoundToggled += ProcessSoundToggled;
        _view.MusicToggled += ProcessMusicToggled;
        _view.FeedbackClicked += ProcessFeedbackClicked;
        _view.PrivacyClicked += ProcessPrivacyPolicyClicked;
        _view.TermsOfUseClicked += ProcessTermsOfUseClicked;
        _privacyPolicy.BackButtonClicked += _view.Enable;
        _termsOfUse.BackButtonClicked += _view.Enable;
        _view.BackButtonClicked += ProcessBackButtonClicked;
    }

    private void OnDisable()
    {
        _mainScreen.SettingsOpened -= _view.Enable;
        _view.SoundToggled -= ProcessSoundToggled;
        _view.MusicToggled -= ProcessMusicToggled;
        _view.FeedbackClicked -= ProcessFeedbackClicked;
        _view.PrivacyClicked -= ProcessPrivacyPolicyClicked;
        _view.TermsOfUseClicked -= ProcessTermsOfUseClicked;
        _privacyPolicy.BackButtonClicked -= _view.Enable;
        _termsOfUse.BackButtonClicked -= _view.Enable;
        _view.BackButtonClicked -= ProcessBackButtonClicked;
    }

    private void ProcessSoundToggled()
    {
        if (_soundOn)
        {
            _view.ToggleOffSoundSprite();
            _audioMixer.audioMixer.SetFloat("Effects", -80f);
            _soundOn = false;
            PlayerPrefs.SetInt("SoundOff", 1);
        }
        else
        {
            _view.ToggleOnSoundSprite();
            _audioMixer.audioMixer.SetFloat("Effects", -20f);
            _soundOn = true;
        }
    }

    private void ProcessMusicToggled()
    {
        if (_musicOn)
        {
            _view.ToggleOffMusicSprite();
            _audioMixer.audioMixer.SetFloat("Music", -80f);
            _musicOn = false;
            PlayerPrefs.SetInt("MusicOff", 1);
        }
        else
        {
            _view.ToggleOnMusicSprite();
            _audioMixer.audioMixer.SetFloat("Music", -20f);
            _musicOn = true;
        }
    }

    private void ProcessFeedbackClicked()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    private void ProcessPrivacyPolicyClicked()
    {
        OpenPrivacyPolicy?.Invoke();
        _view.Disable();
    }

    private void ProcessTermsOfUseClicked()
    {
        OpenTermsOfUse?.Invoke();
        _view.Disable();
    }

    private void ProcessBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}