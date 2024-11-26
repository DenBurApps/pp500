using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipGameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _menuButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MenuButtonClicked;
    
    public string DifficultyText => _difficultyText.text;

    public string TimerText => _timerText.text;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(ProcessMenuButtonClicked);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(ProcessMenuButtonClicked);
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
        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void SetDifficultyText(string text)
    {
        _difficultyText.text = text;
    }
    
    private void ProcessMenuButtonClicked()
    {
        MenuButtonClicked?.Invoke();
    }

}
