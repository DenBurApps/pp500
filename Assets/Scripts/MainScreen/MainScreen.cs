using System;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private MainScreenView _view;
    [SerializeField] private SpeedTapDescriptionView _speedTapDescriptionView;
    [SerializeField] private MemoryFlipDescriptionView _memoryFlipDescriptionView;
    [SerializeField] private ShapeSorterDescriptionView _sorterDescriptionView;
    [SerializeField] private SettingsScreen _settings;

    [Header("Animation Settings")] [SerializeField]
    private float _animationDuration = 0.5f;

    [SerializeField] private float _staggerDelay = 0.1f;

    public event Action SpeedTapOpened;
    public event Action SettingsOpened;
    public event Action MemoryFlipOpened;
    public event Action ShapeSorterOpened;

    private void Start()
    {
        _view.EnableWithAnimation(_animationDuration, _staggerDelay);
    }

    private void OnEnable()
    {
        _speedTapDescriptionView.BackButtonClicked += EnableViewWithAnimation;
        _memoryFlipDescriptionView.BackButtonClicked += EnableViewWithAnimation;
        _sorterDescriptionView.BackButtonClicked += EnableViewWithAnimation;
        _settings.BackButtonClicked += EnableViewWithAnimation;
        _view.SpeedTapClicked += OpenSpeedTapDescription;
        _view.MemoryFlipClicked += OpenMemoryFlipDescription;
        _view.ShapeSorterClicked += OpenShapeSorterDescription;
        _view.SettingClicked += OpenSetting;
    }

    private void OnDisable()
    {
        _speedTapDescriptionView.BackButtonClicked -= EnableViewWithAnimation;
        _memoryFlipDescriptionView.BackButtonClicked -= EnableViewWithAnimation;
        _sorterDescriptionView.BackButtonClicked -= EnableViewWithAnimation;
        _settings.BackButtonClicked -= EnableViewWithAnimation;
        _view.SpeedTapClicked -= OpenSpeedTapDescription;
        _view.MemoryFlipClicked -= OpenMemoryFlipDescription;
        _view.ShapeSorterClicked -= OpenShapeSorterDescription;
        _view.SettingClicked -= OpenSetting;
    }

    private void EnableViewWithAnimation()
    {
        _view.EnableWithAnimation(_animationDuration, _staggerDelay);
    }

    private void OpenSpeedTapDescription()
    {
        _view.DisableWithAnimation(_animationDuration, () => { SpeedTapOpened?.Invoke(); });
    }

    private void OpenMemoryFlipDescription()
    {
        _view.DisableWithAnimation(_animationDuration, () => { MemoryFlipOpened?.Invoke(); });
    }

    private void OpenShapeSorterDescription()
    {
        _view.DisableWithAnimation(_animationDuration, () => { ShapeSorterOpened?.Invoke(); });
    }

    private void OpenSetting()
    {
        _view.DisableWithAnimation(_animationDuration, () => { SettingsOpened?.Invoke(); });
    }
}