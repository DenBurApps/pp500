using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractableObject : MonoBehaviour,IInteractable
{
    [SerializeField] private float _lifetime;

    private float _elapsedTime;
    
    private Button _button;
    
    public event Action<InteractableObject> GotClicked;
    public event Action<InteractableObject> LifetimeEnded;

    private void Awake()
    {
        _button = GetComponent<Button>();
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
            LifetimeEnded?.Invoke(this);
        }
    }
    
    private void ProcessClick()
    {
        GotClicked?.Invoke(this);
    }
}
