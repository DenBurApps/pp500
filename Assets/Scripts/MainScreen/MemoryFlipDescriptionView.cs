using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MemoryFlipDescriptionView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private TMP_Text _easyLevelPassedText;
    [SerializeField] private TMP_Text _normalLevelPassedText;
    [SerializeField] private TMP_Text _hardLevelPassedText;
    [SerializeField] private TMP_Text _youVictoryText;
    [SerializeField] private TMP_Text _youLostText;
    
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
        _mainScreen.MemoryFlipOpened += _screenVisabilityHandler.EnableScreen;
        _backButton.onClick.AddListener(ProcessBackButtonClicked);
        _playButton.onClick.AddListener(ProcessPlayButtonClicked);
    }

    private void OnDisable()
    {
        _mainScreen.MemoryFlipOpened += _screenVisabilityHandler.EnableScreen;
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
       SceneManager.LoadScene("MemoryFlipScene");
    }

    private void SetLevelPassedText()
    {
        if (PlayerPrefs.HasKey("MemoryFlipEasyPassed"))
        {
            _easyLevelPassedText.text = PlayerPrefs.GetInt("MemoryFlipEasyPassed").ToString();
        }
        else
        {
            _easyLevelPassedText.text = "0";
        }
        
        if (PlayerPrefs.HasKey("MemoryFlipNormalPassed"))
        {
            _normalLevelPassedText.text = PlayerPrefs.GetInt("MemoryFlipNormalPassed").ToString();
        }
        else
        {
            _normalLevelPassedText.text = "0";
        }
        
        if (PlayerPrefs.HasKey("MemoryFlipHardPassed"))
        {
            _hardLevelPassedText.text = PlayerPrefs.GetInt("MemoryFlipHardPassed").ToString();
        }
        else
        {
            _hardLevelPassedText.text = "0";
        }
    }

    private void SetStatsText()
    {
        if (PlayerPrefs.HasKey("MemoryFlipWinCount"))
        {
            _youVictoryText.text = PlayerPrefs.GetInt("MemoryFlipWinCount").ToString();
        }
        else
        {
            _youVictoryText.text = "0";
        }

        if (PlayerPrefs.HasKey("MemoryFlipLoseCount"))
        {
            _youLostText.text = PlayerPrefs.GetInt("MemoryFlipLoseCount").ToString();
        }
        else
        {
            _youLostText.text = "0";
        }
    }
    
}
