using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SpeedTapGameView : MonoBehaviour
{
    [Header("UI Buttons")] [SerializeField]
    private Button _menuButton;

    [Header("Text Elements")] [SerializeField]
    private TMP_Text _timerText;

    [SerializeField] private TMP_Text _meteoriteCountText;
    [SerializeField] private TMP_Text _bombCountText;

    [Header("Health Points")] [SerializeField]
    private HPPoint[] _hpPointsImages;

    [Header("Animation Settings")] [SerializeField]
    private float _hpLoseAnimationDuration = 0.5f;

    [SerializeField] private float _hpResetAnimationDuration = 0.7f;
    [SerializeField] private float _countUpdateAnimationDuration = 0.3f;
    [SerializeField] private float _buttonAnimationDuration = 0.5f;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MenuButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        InitializeValidation();
    }

    private void InitializeValidation()
    {
        if (_screenVisabilityHandler == null)
            Debug.LogError("ScreenVisabilityHandler is missing!");

        if (_menuButton == null)
            Debug.LogError("Menu Button is not assigned!");

        if (_hpPointsImages == null || _hpPointsImages.Length == 0)
            Debug.LogWarning("No HP Point Images assigned!");
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
        SetTimerValue(0, 0);
        UpdateBombCount(0);
        UpdateMeteoriteCount(0);
        EnableAllHealthImages();
    }

    public void SetTimerValue(float minutes, float seconds)
    {
        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        AnimateTimerUpdate();
    }

    public string GetTimerText()
    {
        return _timerText.text;
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        AnimateScreenEntrance();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void DecreaceHpImage(int imageIndex)
    {
        if (imageIndex < 0 || imageIndex >= _hpPointsImages.Length)
            return;

        HPPoint hpPoint = _hpPointsImages[imageIndex];
        if (hpPoint?.Image != null)
        {
            hpPoint.SetImageStatus(false);
            AnimateHpPointLoss(hpPoint);
        }
    }

    public void EnableAllHealthImages()
    {
        foreach (var hpPoint in _hpPointsImages)
        {
            if (hpPoint?.Image != null)
            {
                hpPoint.SetImageStatus(true);
            }
        }

        AnimateHpPointReset();
    }

    public void UpdateMeteoriteCount(int count)
    {
        _meteoriteCountText.text = count.ToString();
        AnimateMeteoriteCountUpdate(count);
    }

    public void UpdateBombCount(int count)
    {
        _bombCountText.text = count.ToString();
        AnimateBombCountUpdate(count);
    }

    private void ProcessMenuButtonClicked()
    {
        AnimateButtonClick(_menuButton);
        MenuButtonClicked?.Invoke();
    }

    private void AnimateScreenEntrance()
    {
        Sequence entranceSequence = DOTween.Sequence();

        _menuButton.transform.localScale = Vector3.zero;
        entranceSequence.Append(_menuButton.transform.DOScale(1f, _buttonAnimationDuration).SetEase(Ease.OutBack));

        if (_timerText != null)
        {
            _timerText.transform.localScale = Vector3.zero;
            entranceSequence.Append(_timerText.transform.DOScale(1f, _buttonAnimationDuration).SetEase(Ease.OutBack));
        }

        if (_meteoriteCountText != null)
        {
            _meteoriteCountText.transform.localScale = Vector3.zero;
            entranceSequence.Append(_meteoriteCountText.transform.DOScale(1f, _buttonAnimationDuration)
                .SetEase(Ease.OutBack));
        }

        if (_bombCountText != null)
        {
            _bombCountText.transform.localScale = Vector3.zero;
            entranceSequence.Append(
                _bombCountText.transform.DOScale(1f, _buttonAnimationDuration).SetEase(Ease.OutBack));
        }
    }

    private void AnimateButtonClick(Button button)
    {
        button.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), _buttonAnimationDuration, 1, 0.5f);
    }

    private void AnimateTimerUpdate()
    {
        _timerText.transform.DOKill();

        _timerText.transform.localScale = Vector3.one;

        Sequence timerSequence = DOTween.Sequence();
        timerSequence.Append(_timerText.transform.DOScale(1.1f, _countUpdateAnimationDuration / 2))
            .Append(_timerText.transform.DOScale(1f, _countUpdateAnimationDuration / 2));
    }

    private void AnimateHpPointLoss(HPPoint hpPoint)
    {
        if (hpPoint?.Image == null) return;

        Sequence lossSequence = DOTween.Sequence();

        lossSequence.Append(
            hpPoint.Image.transform.DOShakePosition(_hpLoseAnimationDuration, 10f, 20, 90, false, true));
        lossSequence.Join(hpPoint.Image.transform.DOScale(0f, _hpLoseAnimationDuration).SetEase(Ease.InBack));
        lossSequence.Join(hpPoint.Image.DOFade(0f, _hpLoseAnimationDuration));
    }

    private void AnimateHpPointReset()
    {
        for (int i = 0; i < _hpPointsImages.Length; i++)
        {
            HPPoint hpPoint = _hpPointsImages[i];

            if (hpPoint?.Image == null) continue;

            hpPoint.Image.transform.localScale = Vector3.zero;
            Color imageColor = hpPoint.Image.color;
            hpPoint.Image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);

            hpPoint.Image.transform.DOScale(1.2f, _hpResetAnimationDuration)
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.2f)
                .OnComplete(() => { hpPoint.Image.transform.DOScale(1f, 0.2f); });
        }
    }

    private void AnimateMeteoriteCountUpdate(int count)
    {
        Sequence countSequence = DOTween.Sequence();

        countSequence.Append(_meteoriteCountText.transform.DOPunchScale(
            new Vector3(0.3f, 0.3f, 0.3f),
            _countUpdateAnimationDuration,
            1,
            0.5f
        ));

        Color originalColor = _meteoriteCountText.color;
        countSequence.Join(_meteoriteCountText.DOColor(Color.green, _countUpdateAnimationDuration / 2)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => _meteoriteCountText.color = originalColor));
    }

    private void AnimateBombCountUpdate(int count)
    {
        Sequence countSequence = DOTween.Sequence();

        countSequence.Append(_bombCountText.transform.DOPunchScale(
            new Vector3(0.3f, 0.3f, 0.3f),
            _countUpdateAnimationDuration,
            1,
            0.5f
        ));

        Color originalColor = _bombCountText.color;
        countSequence.Join(_bombCountText.DOColor(Color.red, _countUpdateAnimationDuration / 2)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => _bombCountText.color = originalColor));
    }
}