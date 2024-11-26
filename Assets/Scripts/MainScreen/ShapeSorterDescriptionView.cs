using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShapeSorterDescriptionView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private TMP_Text _easyLevelPassedText;
    [SerializeField] private TMP_Text _normalLevelPassedText;
    [SerializeField] private TMP_Text _hardLevelPassedText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _coinText;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
        SetLevelPassedText();
        SetStatsText();
    }

    public void OnEnable()
    {
        _mainScreen.ShapeSorterOpened += _screenVisabilityHandler.EnableScreen;
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _playButton.onClick.AddListener(ProcessPlayButtonClicked);
    }

    private void OnDisable()
    {
        _mainScreen.ShapeSorterOpened += _screenVisabilityHandler.EnableScreen;
        _backButton.onClick.RemoveListener(ProcessBackButtonClicked);
        _playButton.onClick.RemoveListener(ProcessPlayButtonClicked);
    }

    private void ProcessBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void ProcessPlayButtonClicked()
    {
        SceneManager.LoadScene("ShapeSorterScene");
    }

    private void SetLevelPassedText()
    {
        if (PlayerPrefs.HasKey("ShapeSorterEasyPassed"))
        {
            _easyLevelPassedText.text = PlayerPrefs.GetString("ShapeSorterEasyPassed");
        }
        else
        {
            _easyLevelPassedText.text = "0";
        }

        if (PlayerPrefs.HasKey("ShapeSorterNormalPassed"))
        {
            _normalLevelPassedText.text = PlayerPrefs.GetString("ShapeSorterNormalPassed");
        }
        else
        {
            _normalLevelPassedText.text = "0";
        }

        if (PlayerPrefs.HasKey("ShapeSorterHardPassed"))
        {
            _hardLevelPassedText.text = PlayerPrefs.GetString("ShapeSorterHardPassed");
        }
        else
        {
            _hardLevelPassedText.text = "0";
        }
    }

    private void SetStatsText()
    {
        if (PlayerPrefs.HasKey("ShapeSorterTotalCoins"))
        {
            _coinText.text = PlayerPrefs.GetString("ShapeSorterTotalCoins");
        }
        else
        {
            _coinText.text = "0";
        }

        if (PlayerPrefs.HasKey("ShapeSorterTotalCoins"))
        {
            float time = PlayerPrefs.GetFloat("ShapeSorterTotalTime");
            time += 1;
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);

            _timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }
        else
        {
            _timeText.text = "00:00";
        }
    }
}