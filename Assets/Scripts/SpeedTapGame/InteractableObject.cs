using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private float _lifetime;

    [Header("Animation Settings")]
    [SerializeField] private float _clickScaleDuration = 0.2f;
    [SerializeField] private float _clickScaleAmount = 1.2f;
    [SerializeField] private float _lifetimeEndFadeDuration = 0.3f;

    private float _elapsedTime;
    
    private Button _button;
    private Image _image;
    
    public event Action<InteractableObject> GotClicked;
    public event Action<InteractableObject> LifetimeEnded;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ProcessClick);
        _elapsedTime = 0f;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ProcessClick);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _lifetime)
        {
            AnimateLifetimeEnd();
        }
    }
    
    private void ProcessClick()
    {
        Sequence clickSequence = DOTween.Sequence();

        clickSequence.Append(transform.DOScale(_clickScaleAmount, _clickScaleDuration / 2)
            .SetEase(Ease.OutQuad));
        clickSequence.Append(transform.DOScale(1f, _clickScaleDuration / 2)
            .SetEase(Ease.InQuad));

        clickSequence.Join(transform.DORotate(new Vector3(0, 0, 15), _clickScaleDuration / 2)
            .SetEase(Ease.OutQuad));
        clickSequence.Append(transform.DORotate(Vector3.zero, _clickScaleDuration / 2)
            .SetEase(Ease.InQuad));

        clickSequence.OnComplete(() => 
        {
            GotClicked?.Invoke(this);
        });
    }

    private void AnimateLifetimeEnd()
    {
        Sequence endSequence = DOTween.Sequence();

        if (_image != null)
        {
            endSequence.Append(_image.DOFade(0f, _lifetimeEndFadeDuration));
        }

        endSequence.Join(transform.DOScale(0f, _lifetimeEndFadeDuration));

        endSequence.OnComplete(() => 
        {
            LifetimeEnded?.Invoke(this);
            Destroy(gameObject);
        });
    }
}