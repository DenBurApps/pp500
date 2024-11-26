using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class Figure : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private int _easySpeed;
    [SerializeField] private int _normalSpeed;
    [SerializeField] private int _hardSpeed;
    [SerializeField] private Vector3 _bottonPosition;

    private CanvasGroup _canvasGroup;
    private Canvas _parentCanvas;
    private RectTransform _rectTransform;
    private Vector2 _beforeDragPosition;
    private IEnumerator _movingCoroutine;
    private int _currentSpeed;

    public event Action<Figure> ReachedBottom;
    
    public RectTransform RectTransform => _rectTransform;

    public FigureTypes Type { get; protected set; }

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
    }

    private void OnDisable()
    {
        DisableMovement();
    }

    public void SetParentCanvas(Canvas canvas)
    {
        _parentCanvas = canvas;
    }

    public void ReturnToPreviousPosition()
    {
        _rectTransform.anchoredPosition = _beforeDragPosition;

        EnableMovement();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beforeDragPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;

        DisableMovement();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

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
        if (_movingCoroutine == null)
            _movingCoroutine = StartMoving();

        StartCoroutine(_movingCoroutine);
    }

    public void DisableMovement()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }

    private IEnumerator StartMoving()
    {
        while (true)
        {
            _rectTransform.position += Vector3.down * Time.deltaTime * _currentSpeed;

            if (_rectTransform.position.y <= _bottonPosition.y)
            {
                DisableMovement();
                ReachedBottom?.Invoke(this);
            }
                
            
            yield return null;
        }
    }
}