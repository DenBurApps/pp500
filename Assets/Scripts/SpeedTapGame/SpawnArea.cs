using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RectTransform))]
public class SpawnArea : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public Vector2 GetRandomPositionToSpawn()
    {
        Vector3[] worldCorners = new Vector3[4];
        _rectTransform.GetWorldCorners(worldCorners);
        
        Vector3 minPosition = worldCorners[0];
        Vector3 maxPosition = worldCorners[2];
        
        float randomX = Random.Range(minPosition.x, maxPosition.x);
        float randomY = Random.Range(minPosition.y, maxPosition.y);

        return new Vector2(randomX, randomY);
    }
}
