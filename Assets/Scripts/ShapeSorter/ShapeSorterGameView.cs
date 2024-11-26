using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterGameView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _coinsAmount;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _squareQuantity;
    [SerializeField] private TMP_Text _triangleQuantity;
    [SerializeField] private TMP_Text _circleQuantity;
    [SerializeField] private TMP_Text _rectangleQuantity;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MenuButtonClicked;

    public TMP_Text CoinsAmount => _coinsAmount;

    public TMP_Text SquareQuantity => _squareQuantity;

    public TMP_Text TriangleQuantity => _triangleQuantity;

    public TMP_Text CircleQuantity => _circleQuantity;

    public TMP_Text RectangleQuantity => _rectangleQuantity;

    public string TimerText => _timer.text;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void OnEnable()
    {
        _menuButton.onClick.AddListener(ProcessMenuClicked);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(ProcessMenuClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void SetTimerValue(float minutes, float seconds)
    {
        _timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void SetTextAmount(int amount, TMP_Text textToSet)
    {
        textToSet.text = amount.ToString();
    }

    private void ProcessMenuClicked()
    {
        MenuButtonClicked?.Invoke();
    }
    
}
