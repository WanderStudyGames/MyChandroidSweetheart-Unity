using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wardrobe", menuName = "ScriptableObjects/Clothings/Wardrobe")]
public class Wardrobe : ScriptableObject
{
    [field: SerializeField] public SavedScriptableObjectList<Clothing> Clothings { get; private set; }
    [field: SerializeField] public SavedScriptableObjectList<Clothing> Blueprints { get; private set; }
    [field: SerializeField] public List<Clothing> DefaultClothings { get; private set; } = new();
    [field: SerializeField] public List<Clothing> DefaultBlueprints { get; private set; } = new();
    [SerializeField] private GameEvent _clothingAddedGameEvent;
    [SerializeField] private GameEvent _blueprintAddedGameEvent;
    private void OnEnable()
    {
        Clothings = new("wardrobe", files => files.companionData, DefaultClothings);
        Blueprints = new("blueprints", files => files.companionData, DefaultBlueprints);
    }
    public void AddClothings(List<Clothing> clothings)
    {
        bool newAdded = false;
        foreach (Clothing clothing in clothings)
        {

            if (Clothings.Add(clothing))
            {
                newAdded = true;
            }
        }
        if (newAdded)
        {
            _clothingAddedGameEvent.Raise();
        }
    }
    public void AddBlueprints(List<Clothing> clothings)
    {
        bool newAdded = false;
        foreach (Clothing clothing in clothings)
        {

            if (Blueprints.Add(clothing))
            {
                newAdded = true;
            }
        }
        if (newAdded)
        {
            _blueprintAddedGameEvent.Raise();
        }
    }
}
