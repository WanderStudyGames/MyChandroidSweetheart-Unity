using UnityEngine;

[CreateAssetMenu(fileName = "ColorPallette", menuName = "ScriptableObjects/ColorPallette")]
public class ColorPallette : ScriptableObject
{
    [field: SerializeField] public Color[] Swatches { get; private set; }
}