using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HPPoint : MonoBehaviour
{
    private Image _image;

    public Image Image => _image;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetImageStatus(bool status)
    {
        _image.enabled = status;
    }
}