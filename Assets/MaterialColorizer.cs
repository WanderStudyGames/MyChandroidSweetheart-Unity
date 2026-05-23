using UnityEngine;

public class MaterialColorizer : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _materialIndex;
    [SerializeField] private bool _colorizeOnAwake = true;
    private void Awake()
    {
        if (_colorizeOnAwake)
            Colorize(_renderer);
    }
    public void Colorize(Renderer renderer)
    {
        renderer.materials[_materialIndex].color = _color;
        renderer.materials[_materialIndex].SetToonColor(_color);
    }
}
