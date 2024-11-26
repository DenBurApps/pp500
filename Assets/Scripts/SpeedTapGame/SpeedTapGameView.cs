using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SpeedTapGameView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _meteoriteCountText;
    [SerializeField] private TMP_Text _bombCountText;
    [SerializeField] private HPPoint[] _hpPointsImages;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action MenuButtonClicked;

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

    public void ResetAllValues()
    {
        SetTimerValue(0,0);
        UpdateBombCount(0);
        UpdateMeteoriteCount(0);
        EnableAllHealthImages();
    }

    public void SetTimerValue(float minutes, float seconds)
    {
        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public string GetTimerText()
    {
        return _timerText.text;
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void DecreaceHpImage(int imageIndex)
    {
        if(imageIndex < 0)
            return;
        
        _hpPointsImages[imageIndex].SetImageStatus(false);
    }

    public void EnableAllHealthImages()
    {
        foreach (var hpPoint in _hpPointsImages)
        {
            hpPoint.SetImageStatus(true);
        }
    }

    public void UpdateMeteoriteCount(int count)
    {
        _meteoriteCountText.text = count.ToString();
    }

    public void UpdateBombCount(int count)
    {
        _bombCountText.text = count.ToString();
    }

    private void ProcessMenuButtonClicked()
    {
        MenuButtonClicked?.Invoke();
    }
}
