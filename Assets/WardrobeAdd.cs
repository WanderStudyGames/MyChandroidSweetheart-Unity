using System;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeAdd : MonoBehaviour
{
    [SerializeField] private Wardrobe _wardrobe;
    [SerializeField] private List<Clothing> _clothings;
    public static event Action OnBlueprintAdded;
    public void AddClothings()
    {
        _wardrobe.AddBlueprints(_clothings);
    }
}
