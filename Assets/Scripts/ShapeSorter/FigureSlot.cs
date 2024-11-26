using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FigureSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private FigureTypes _figureType;

    public event Action<Figure> FigureInsertedCorrectly;
    public event Action<Figure> FigureInsertedIncorrectly;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            VerifyFigure(eventData.pointerDrag.GetComponent<Figure>());
        }
    }

    public void ToggleActiveStatus(bool status)
    {
        gameObject.SetActive(status);
    }

    private void VerifyFigure(Figure figure)
    {
        if (figure.Type == _figureType)
        {
            FigureInsertedCorrectly?.Invoke(figure);
        }
        else
        {
            FigureInsertedIncorrectly?.Invoke(figure);
        }
    }
}