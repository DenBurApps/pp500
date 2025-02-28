using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class Figure : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private int _easySpeed;
    [SerializeField] private int _normalSpeed;
    [SerializeField] private int _hardSpeed;
    [SerializeField] private Vector3 _bottonPosition;
    [SerializeField] private float _returnDuration = 0.3f;
    [SerializeField] private Ease _returnEase = Ease.OutBack;
    [SerializeField] private float _dragScaleMultiplier = 1.1f;
    [SerializeField] private float _scaleAnimDuration = 0.2f;
    [SerializeField] private float _rotationAmount = 5f;

    private CanvasGroup _canvasGroup;
    private Canvas _parentCanvas;
    private RectTransform _rectTransform;
    private Vector2 _beforeDragPosition;
    private Vector3 _originalScale;
    private IEnumerator _movingCoroutine;
    private int _currentSpeed;
    private Sequence _moveSequence;

    public event Action<Figure> ReachedBottom;
    
    public RectTransform RectTransform => _rectTransform;

    public FigureTypes Type { get; protected set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        if (_originalScale == Vector3.zero)
        {
            _originalScale = transform.localScale;
        }

        // Make sure figures are visible immediately but with entry animation
        transform.localScale = _originalScale * 0.8f;
        transform.DOScale(_originalScale, 0.4f).SetEase(Ease.OutBack);
        
        // Only play the pulse animation if we're not in the middle of movement
        if (_moveSequence == null)
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .Append(transform.DOScale(_originalScale * 1.05f, 0.3f))
                .Append(transform.DOScale(_originalScale, 0.3f));
        }
    }

    private void OnDisable()
    {
        DisableMovement();
        DOTween.Kill(transform);
    }

    public void SetParentCanvas(Canvas canvas)
    {
        _parentCanvas = canvas;
    }

    public void ReturnToPreviousPosition()
    {
        _rectTransform.DOAnchorPos(_beforeDragPosition, _returnDuration)
            .SetEase(_returnEase)
            .OnComplete(() => EnableMovement());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(_originalScale * 1.05f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beforeDragPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;

        transform.DOScale(_originalScale * _dragScaleMultiplier, _scaleAnimDuration);

        DisableMovement();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        transform.DOScale(_originalScale, _scaleAnimDuration);

        ReturnToPreviousPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_parentCanvas != null)
        {
            _rectTransform.anchoredPosition += eventData.delta / _parentCanvas.scaleFactor;
        }
    }

    public void SetDifficultySpeed(DifficultyTypes difficultyType)
    {
        if (difficultyType == DifficultyTypes.Easy)
            _currentSpeed = _easySpeed;
        else if (difficultyType == DifficultyTypes.Normal)
            _currentSpeed = _normalSpeed;
        else if (difficultyType == DifficultyTypes.Hard)
            _currentSpeed = _hardSpeed;
    }

    public void EnableMovement()
    {
        DisableMovement();

        // Always use the coroutine method as it was working before
        _movingCoroutine = StartMoving();
        StartCoroutine(_movingCoroutine);
    }

    public void DisableMovement()
    {
        if (_moveSequence != null)
        {
            _moveSequence.Kill();
            _moveSequence = null;
        }
        
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }

        transform.DORotate(Vector3.zero, 0.2f);
    }

    private IEnumerator StartMoving()
    {
        // Ensures we have a valid speed
        if (_currentSpeed <= 0)
        {
            if (_normalSpeed > 0)
                _currentSpeed = _normalSpeed;
            else
                _currentSpeed = 100;
        }

        while (true)
        {
            _rectTransform.position += Vector3.down * Time.deltaTime * _currentSpeed;

            if (_rectTransform.position.y <= _bottonPosition.y)
            {
                DisableMovement();
                ReachedBottom?.Invoke(this);
                break;
            }
            
            yield return null;
        }
    }
}