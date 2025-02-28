using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FigureSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private FigureTypes _figureType;
    [SerializeField] private float _correctInsertionAnimDuration = 0.5f;
    [SerializeField] private float _incorrectInsertionAnimDuration = 0.3f;
    [SerializeField] private Color _highlightColor = Color.green;
    [SerializeField] private Color _errorColor = Color.red;
    [SerializeField] private Color _normalColor = Color.white;

    private Vector3 _originalScale;
    private SpriteRenderer _spriteRenderer;
    private CanvasGroup _canvasGroup;

    public event Action<Figure> FigureInsertedCorrectly;
    public event Action<Figure> FigureInsertedIncorrectly;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_spriteRenderer == null)
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                _normalColor = image.color;
            }
        }
        else
        {
            _normalColor = _spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        DOTween.Kill(transform);
        DOTween.Kill(gameObject);
        
        if (_originalScale == Vector3.zero)
        {
            _originalScale = transform.localScale;
        }
        
        transform.localScale = _originalScale;
    }

    private void OnDisable()
    {
        DOTween.Kill(gameObject);
        DOTween.Kill(transform);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Figure figure = eventData.pointerDrag.GetComponent<Figure>();
            if (figure != null)
            {
                VerifyFigure(figure);
            }
        }
    }

    public void ToggleActiveStatus(bool status)
    {
        gameObject.SetActive(status);
        
        if (status)
        {
            transform.localScale = _originalScale;
        }
    }

    private void VerifyFigure(Figure figure)
    {
        DOTween.Kill(transform);
        DOTween.Kill(gameObject);
        
        transform.localScale = _originalScale;
        
        if (figure.Type == _figureType)
        {
            PlayCorrectInsertionAnimation(figure);
            FigureInsertedCorrectly?.Invoke(figure);
        }
        else
        {
            PlayIncorrectInsertionAnimation(figure);
            FigureInsertedIncorrectly?.Invoke(figure);
        }
    }

    private void PlayCorrectInsertionAnimation(Figure figure)
    {
        DOTween.Kill(transform);
        DOTween.Kill(gameObject);
        
        if (figure != null)
        {
            DOTween.Kill(figure.transform);
            figure.transform.DOMove(transform.position, 0.3f)
                .SetEase(Ease.OutBack);
        }

        transform.DOScale(_originalScale * 1.2f, 0.15f)
            .OnComplete(() => {
                if (this != null && gameObject != null && gameObject.activeInHierarchy)
                {
                    transform.DOScale(_originalScale, 0.15f);
                }
            });

        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOColor(_highlightColor, 0.15f)
                .OnComplete(() => {
                    if (this != null && gameObject != null && gameObject.activeInHierarchy && _spriteRenderer != null)
                    {
                        _spriteRenderer.DOColor(_normalColor, 0.15f);
                    }
                });
        }
        else
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.DOColor(_highlightColor, 0.15f)
                    .OnComplete(() => {
                        if (this != null && gameObject != null && gameObject.activeInHierarchy && image != null)
                        {
                            image.DOColor(_normalColor, 0.15f);
                        }
                    });
            }
        }
    }

    private void PlayIncorrectInsertionAnimation(Figure figure)
    {
        DOTween.Kill(transform);
        DOTween.Kill(gameObject);
        
        Vector3 originalPosition = transform.position;
        transform.DOShakePosition(0.2f, 3f, 10, 0f, false, false)
            .OnComplete(() => {
                if (this != null && gameObject != null && gameObject.activeInHierarchy)
                {
                    transform.position = originalPosition;
                }
            });
        
        if (_spriteRenderer != null)
        {
            Color originalColor = _spriteRenderer.color;
            _spriteRenderer.DOColor(_errorColor, 0.1f)
                .OnComplete(() => {
                    if (this != null && gameObject != null && gameObject.activeInHierarchy && _spriteRenderer != null)
                    {
                        _spriteRenderer.DOColor(originalColor, 0.1f);
                    }
                });
        }
        else
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                Color originalColor = image.color;
                image.DOColor(_errorColor, 0.1f)
                    .OnComplete(() => {
                        if (this != null && gameObject != null && gameObject.activeInHierarchy && image != null)
                        {
                            image.DOColor(originalColor, 0.1f);
                        }
                    });
            }
        }
    }
}