using System;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private MainScreenView _view;
    [SerializeField] private SpeedTapDescriptionView _speedTapDescriptionView;
    [SerializeField] private MemoryFlipDescriptionView _memoryFlipDescriptionView;
    [SerializeField] private ShapeSorterDescriptionView _sorterDescriptionView;
    [SerializeField] private SettingsScreen _settings;
    
    public event Action SpeedTapOpened;
    public event Action SettingsOpened;
    public event Action MemoryFlipOpened;
    public event Action ShapeSorterOpened;
    
    private void Start()
    {
        _view.Enable();
    }

    private void OnEnable()
    {
        _speedTapDescriptionView.BackButtonClicked += _view.Enable;
        _memoryFlipDescriptionView.BackButtonClicked += _view.Enable;
        _sorterDescriptionView.BackButtonClicked += _view.Enable;
        _settings.BackButtonClicked += _view.Enable;
        _view.SpeedTapClicked += OpenSpeedTapDescription;
        _view.MemoryFlipClicked += OpenMemoryFlipDescription;
        _view.ShapeSorterClicked += OpenShapeSorterDescription;
        _view.SettingClicked += OpenSetting;

    }

    private void OnDisable()
    {
        _speedTapDescriptionView.BackButtonClicked -= _view.Enable;
        _memoryFlipDescriptionView.BackButtonClicked -= _view.Enable;
        _sorterDescriptionView.BackButtonClicked -= _view.Enable;
        _settings.BackButtonClicked -= _view.Enable;
        _view.SpeedTapClicked -= OpenSpeedTapDescription;
        _view.MemoryFlipClicked -= OpenMemoryFlipDescription;
        _view.ShapeSorterClicked -= OpenShapeSorterDescription;
        _view.SettingClicked -= OpenSetting;
    }

    private void OpenSpeedTapDescription()
    {
        SpeedTapOpened?.Invoke();
        _view.Disable();
    }

    private void OpenMemoryFlipDescription()
    {
        MemoryFlipOpened?.Invoke();
        _view.Disable();
    }

    private void OpenShapeSorterDescription()
    {
        ShapeSorterOpened?.Invoke();
        _view.Disable();
    }

    private void OpenSetting()
    {
        SettingsOpened?.Invoke();
        _view.Disable();
    }
    
}
