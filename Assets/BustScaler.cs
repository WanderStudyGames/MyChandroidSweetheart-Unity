using System.Collections;
using UnityEngine;

public class BustScaler : MonoBehaviour
{
    [SerializeField] private Transform _buttTransform;
    private void OnEnable()
    {
        UpdateScale();
        BustSlider.OnValueChanged += UpdateScale;
        SaveSystem.OnLoadFile += Load;
    }
    private void Load(SaveSystem.SaveFileNames files)
    {
        StartCoroutine(OneFrame());
        IEnumerator OneFrame()
        {
            yield return new WaitForEndOfFrame();
            UpdateScale();
        }
    }
    private void OnDisable()
    {
        BustSlider.OnValueChanged -= UpdateScale;
        SaveSystem.OnLoadFile -= Load;
    }
    private void UpdateScale()
    {
        transform.localScale = Vector3.one * BustSlider.Value;
        _buttTransform.localScale = Vector3.one * BustSlider.ButtValue;
    }
}
