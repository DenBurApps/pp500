using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterDifficultySelection : MonoBehaviour
{
    [SerializeField] private ShapeSorterMainMenu _mainMenu;
    [SerializeField] private Button _easyButton;
    [SerializeField] private Button _normalButton;
    [SerializeField] private Button _hardButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private ShapeSorterGameController _gameController;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action EasySelected;
    public event Action NormalSelected;
    public event Action HardSelected;
    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void OnEnable()
    {
        _mainMenu.NewGame += _screenVisabilityHandler.EnableScreen;
        _easyButton.onClick.AddListener(ProcessEasyButtonSelected);
        _normalButton.onClick.AddListener(ProcessNormalButtonSelected);
        _hardButton.onClick.AddListener(ProcessHardButtonSelected);
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _gameController.GameRestarted += _screenVisabilityHandler.EnableScreen;
    }

    private void OnDisable()
    {
        _mainMenu.NewGame -= _screenVisabilityHandler.EnableScreen;
        _easyButton.onClick.RemoveListener(ProcessEasyButtonSelected);
        _normalButton.onClick.RemoveListener(ProcessNormalButtonSelected);
        _hardButton.onClick.RemoveListener(ProcessHardButtonSelected);
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _gameController.GameRestarted -= _screenVisabilityHandler.EnableScreen;
    }
    
    private void ProcessEasyButtonSelected()
    {
        EasySelected?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessNormalButtonSelected()
    {
        NormalSelected?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessHardButtonSelected()
    {
        HardSelected?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}
