using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class FishCell : MonoBehaviour
{
    [SerializeField] private Image _emptyImage;
    [SerializeField] private FishCellSpriteProvider _spriteProvider;
    
    private Image _image;
    private Button _interactButton;
    private bool _isFliped;
    private FishTypes _currentType;
    
    public event Action<FishCell> Clicked;

    public FishTypes CurrentType => _currentType;
    public bool IsFliped => _isFliped;

    private void Awake()
    {
        _interactButton = GetComponent<Button>();
        _image = GetComponent<Image>();
        
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
        _emptyImage.enabled = false;
        _isFliped = true;
        _interactButton.onClick.RemoveListener(ProcessClick);
    }

    public void HideFishImage()
    {
        _emptyImage.enabled = true;
        _isFliped = false;
        _interactButton.onClick.AddListener(ProcessClick);
    }

    public void Disable()
    {
        _interactButton.interactable = false;
        _image.enabled = false;
        _emptyImage.enabled = false;
        
    }

    public void ReturnToDefault()
    {
        _emptyImage.enabled = true;
        _interactButton.interactable = true;
        _image.enabled = true;
        _interactButton.onClick.AddListener(ProcessClick);
        _isFliped = false;
    }

    private void ProcessClick()
    {
        Clicked?.Invoke(this);
    }
    
}
