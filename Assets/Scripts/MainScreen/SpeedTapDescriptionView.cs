using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SpeedTapDescriptionView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private TMP_Text _destroyedMeteoritesText;
    [SerializeField] private TMP_Text _longestLevelText;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
        SetMeteoritesText();
        SetLongestLevelText();
    }

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void OnEnable()
    {
        _mainScreen.SpeedTapOpened += _screenVisabilityHandler.EnableScreen;
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _playButton.onClick.AddListener(ProcessPlayButtonClicked);
    }

    private void OnDisable()
    {
        _mainScreen.SpeedTapOpened += _screenVisabilityHandler.EnableScreen;
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _playButton.onClick.RemoveListener(ProcessPlayButtonClicked);
    }

    private void ProcessPlayButtonClicked()
    {
        SceneManager.LoadScene("SpeedTapScene");
    }

    private void ProcessBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void SetMeteoritesText()
    {
        if (PlayerPrefs.HasKey("TotalMeteoritesDestroyed"))
        {
            _destroyedMeteoritesText.text = PlayerPrefs.GetInt("TotalMeteoritesDestroyed").ToString();
        }
        else
        {
            _destroyedMeteoritesText.text = "0";
        }
    }

    private void SetLongestLevelText()
    {
        if (PlayerPrefs.HasKey("ShapeSorterTotalTime"))
        {
            _longestLevelText.text = PlayerPrefs.GetString("SpeedTapMaxSurvivalTimeText");
        }
        else
        {
            _longestLevelText.text = "00:00";
        }
    }
}
