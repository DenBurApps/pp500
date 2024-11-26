using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSorterSpawnArea : MonoBehaviour
{
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _yPosition;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public Vector2 GetRandomXPositionToSpawn()
    {
        /*Vector3[] worldCorners = new Vector3[4];
        _rectTransform.GetWorldCorners(worldCorners);
        
        Vector3 minPosition = worldCorners[0];
        Vector3 maxPosition = worldCorners[2];
        
        float marginOffset = (maxPosition.x - minPosition.x) * _horizontalMarginPercent;
        float minX = minPosition.x + marginOffset;
        float maxX = maxPosition.x - marginOffset;*/
        
        // Generate a random X position within the new bounds
        float randomX = Random.Range(_minX, _maxX);
        
        return new Vector2(randomX, _yPosition);
    }
    
}
