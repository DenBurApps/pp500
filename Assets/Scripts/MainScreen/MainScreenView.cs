using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _speedTapButton;
    [SerializeField] private Button _memoryFlipButton;
    [SerializeField] private Button _shapeSorterButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action SettingClicked;
    public event Action SpeedTapClicked;
    public event Action MemoryFlipClicked;
    public event Action ShapeSorterClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(ProcessSettingsClicked);
        _speedTapButton.onClick.AddListener(ProcessSpeedTapClicked);
        _memoryFlipButton.onClick.AddListener(ProcessMemoryFlipClicked);
        _shapeSorterButton.onClick.AddListener(ProcessShapeSorterClicked);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(ProcessSettingsClicked);
        _speedTapButton.onClick.RemoveListener(ProcessSpeedTapClicked);
        _memoryFlipButton.onClick.RemoveListener(ProcessMemoryFlipClicked);
        _shapeSorterButton.onClick.RemoveListener(ProcessShapeSorterClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessShapeSorterClicked()
    {
        ShapeSorterClicked?.Invoke();
    }

    private void ProcessMemoryFlipClicked()
    {
        MemoryFlipClicked?.Invoke();
    }

    private void ProcessSpeedTapClicked()
    {
        SpeedTapClicked?.Invoke();
    }

    private void ProcessSettingsClicked()
    {
        SettingClicked?.Invoke();
    }
}
