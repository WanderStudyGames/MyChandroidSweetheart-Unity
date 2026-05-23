using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] private Wardrobe _wardrobe;
    [field: SerializeField] public Outfit _outfit { get; private set; }
    public List<Clothing> _hairstyles { get; private set; } = new();
    public List<Clothing> _tops { get; private set; } = new();
    public List<Clothing> _bottoms { get; private set; } = new();
    public List<Clothing> _accessories { get; private set; } = new();

    private void SortClothings()
    {
        _hairstyles = new();
        _tops = new();
        _bottoms = new();
        _accessories = new();
        foreach (var cl in _wardrobe.Clothings.GetValues())
        {
            switch (cl.Type)
            {
                case Clothing.ClothingType.Top:
                    _tops.Add(cl);
                    break;
                case Clothing.ClothingType.Bottom:
                    _bottoms.Add(cl);
                    break;
                case Clothing.ClothingType.Hair:
                    _hairstyles.Add(cl);
                    break;
                case Clothing.ClothingType.Accessory:
                    _accessories.Add(cl);
                    break;
            }
        }
    }
    private void Awake()
    {
        SortClothings();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
