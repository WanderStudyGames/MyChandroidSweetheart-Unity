using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _material;

    public void SetColor(Color color)
    {
        _material.SetToonColor(color);
    }
}
