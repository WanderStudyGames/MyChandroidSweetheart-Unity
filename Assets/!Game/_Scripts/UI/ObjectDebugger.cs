using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class ObjectDebugger : MonoBehaviour
{
    [Dependency] [SerializeField] ScriptableObject _object;
    [Dependency] [SerializeField] private TextMeshProUGUI tmp;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    [ExecuteAlways]
    private void OnEnable()
    {
        SetText();
    }
    [ExecuteAlways]
    [ContextMenu("Bleh")]
    private void SetText()
    {
        tmp.text = "";
        foreach (PropertyInfo a in _object.GetType().GetProperties())
        {
            tmp.text += $"{a.Name}: {a.GetValue(_object)}\n";
        }
    }
    private void Update()
    {
        SetText();
    }
}
