using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCellTypeProvider
{
    private readonly int _pairsCount = 10;
    private readonly FishTypes[] _allFishTypes = (FishTypes[])System.Enum.GetValues(typeof(FishTypes));
    
    public List<FishTypes> GetFishPairs()
    {
        List<FishTypes> selectedFishTypes = new List<FishTypes>();
        List<FishTypes> fishPairs = new List<FishTypes>();
        
        while (selectedFishTypes.Count < _pairsCount)
        {
            FishTypes randomFish = _allFishTypes[Random.Range(0, _allFishTypes.Length)];
            
            if (!selectedFishTypes.Contains(randomFish))
            {
                selectedFishTypes.Add(randomFish);
            }
        }
        
        foreach (FishTypes fish in selectedFishTypes)
        {
            fishPairs.Add(fish);
            fishPairs.Add(fish);
        }
        
        ShuffleList(fishPairs);

        return fishPairs;
    }

    private void ShuffleList(List<FishTypes> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            FishTypes temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        } 
    }
}

public enum FishTypes
{
    RedYellow,
    PurpleYellow,
    GreenPurple,
    RedPurple,
    YellowGreen,
    RedBlue,
    BlueYellow,
    YellowYellow,
    PurpleRed,
    StripedYellow
}
