using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenVisabilityHandler : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }

    public void EnableScreen()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        //gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
       //gameObject.SetActive(false);
    }

    public Tween FadeIn(float duration)
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        return _canvasGroup.DOFade(1, duration)
            .OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            });
    }

    public Tween FadeOut(float duration)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        return _canvasGroup.DOFade(0, duration)
            .OnComplete(() => gameObject.SetActive(false));
    }
}