using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCellSpriteProvider : MonoBehaviour
{
    [SerializeField] private Sprite _redYeelow;
    [SerializeField] private Sprite _purpleYellow;
    [SerializeField] private Sprite _greenPurple;
    [SerializeField] private Sprite _redPurple;
    [SerializeField] private Sprite _yellowGreen;
    [SerializeField] private Sprite _redBlue;
    [SerializeField] private Sprite _blueYellow;
    [SerializeField] private Sprite _yellowYellow;
    [SerializeField] private Sprite _purpleRed;
    [SerializeField] private Sprite _stripedYellow;

    [SerializeField] private Sprite _emptySprite;

    public Sprite GetExactSprite(FishTypes type)
    {
        switch (type)
        {
            case(FishTypes.RedYellow):
                return _redYeelow;
            case(FishTypes.PurpleYellow):
                return _purpleYellow;
            case(FishTypes.GreenPurple):
                return _greenPurple;
            case (FishTypes.RedPurple):
                return _redPurple;
            case (FishTypes.YellowGreen):
                return _yellowGreen;
            case (FishTypes.RedBlue):
                return _redBlue;
            case (FishTypes.BlueYellow):
                return _blueYellow;
            case (FishTypes.YellowYellow):
                return _yellowYellow;
            case (FishTypes.PurpleRed):
                return _purpleRed;
            case (FishTypes.StripedYellow):
                return _stripedYellow;
        }

        return _emptySprite;
    }
}
