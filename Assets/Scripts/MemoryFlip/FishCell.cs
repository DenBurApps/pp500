using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class FishCell : MonoBehaviour
{
    [SerializeField] private Image _emptyImage;
    [SerializeField] private FishCellSpriteProvider _spriteProvider;
    
    [SerializeField] private float _flipDuration = 0.3f;
    [SerializeField] private float _matchedDisappearDuration = 0.5f;
    [SerializeField] private Ease _flipEase = Ease.OutQuad;
    [SerializeField] private Ease _disappearEase = Ease.InBack;
    
    private Image _image;
    private Button _interactButton;
    private bool _isFliped;
    private FishTypes _currentType;
    private RectTransform _rectTransform;
    private Coroutine _currentAnimation;
    
    public event Action<FishCell> Clicked;

    public FishTypes CurrentType => _currentType;
    public bool IsFliped => _isFliped;

    private void Awake()
    {
        _interactButton = GetComponent<Button>();
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        
        ReturnToDefault();
    }

    private void Start()
    {
        HideFishImage();
    }

    private void OnEnable()
    {
        _interactButton.onClick.AddListener(ProcessClick);
    }

    private void OnDisable()
    {
        _interactButton.onClick.RemoveListener(ProcessClick);
    }

    public void SetRandomFishType(FishTypes type)
    {
        _currentType = type;
        _image.sprite = _spriteProvider.GetExactSprite(_currentType);
    }

    public void ShowFishImage()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }
        
        _emptyImage.enabled = false;
        _isFliped = true;
        _interactButton.onClick.RemoveListener(ProcessClick);
        
        _currentAnimation = StartCoroutine(FlipAnimation(true));
    }

    public void HideFishImage()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }
        
        _emptyImage.enabled = true;
        _isFliped = false;
        
        if (!_interactButton.onClick.GetPersistentEventCount().Equals(0))
        {
            _interactButton.onClick.RemoveListener(ProcessClick);
        }
        
        _interactButton.onClick.AddListener(ProcessClick);
        
        _currentAnimation = StartCoroutine(FlipAnimation(false));
    }

    public void Disable()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }
        
        _currentAnimation = StartCoroutine(DisappearAnimation());
    }

    public void ReturnToDefault()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }
        
        _rectTransform.localScale = Vector3.one;
        _emptyImage.enabled = true;
        _interactButton.interactable = true;
        _image.enabled = true;
        
        if (!_interactButton.onClick.GetPersistentEventCount().Equals(0))
        {
            _interactButton.onClick.RemoveListener(ProcessClick);
        }
        
        _interactButton.onClick.AddListener(ProcessClick);
        _isFliped = false;
    }

    private void ProcessClick()
    {
        Clicked?.Invoke(this);
    }
    
    private IEnumerator FlipAnimation(bool showFish)
    {
        _rectTransform.DOScaleX(0, _flipDuration / 2).SetEase(_flipEase);
        
        yield return new WaitForSeconds(_flipDuration / 2);
        
        _rectTransform.DOScaleX(1, _flipDuration / 2).SetEase(_flipEase);
        
        yield return new WaitForSeconds(_flipDuration / 2);
        
        _currentAnimation = null;
    }
    
    private IEnumerator DisappearAnimation()
    {
        _interactButton.interactable = false;
        _image.enabled = true;
        _emptyImage.enabled = false;
        
        _rectTransform.DOScale(0.1f, _matchedDisappearDuration).SetEase(_disappearEase);
        _image.DOFade(0, _matchedDisappearDuration).SetEase(_disappearEase);
        
        yield return new WaitForSeconds(_matchedDisappearDuration);
        
        _image.enabled = false;
        _emptyImage.enabled = false;
        
        _currentAnimation = null;
    }
}