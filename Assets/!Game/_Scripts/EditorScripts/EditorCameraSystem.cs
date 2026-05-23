using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class EditorCameraSystem : MonoBehaviour
{
    private List<Camera> _cameras = new();
    private List<KeyControl> _keys = new();
    void Awake()
    {
        _cameras.Clear();
        _cameras = GetComponentsInChildren<Camera>(false).ToList();
        var k = Keyboard.current;
        _keys.Clear();
        _keys.Add(k.digit1Key);
        _keys.Add(k.digit2Key);
        _keys.Add(k.digit3Key);
        _keys.Add(k.digit4Key);
        _keys.Add(k.digit5Key);
        _keys.Add(k.digit6Key);
        _keys.Add(k.digit7Key);
        _keys.Add(k.digit8Key);
        _keys.Add(k.digit9Key);
        _keys.Add(k.digit0Key);
        _keys.Add(k.minusKey);
        _keys.Add(k.equalsKey);
    }
    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        DisableAll();
    }

    private void SetCamera(int i)
    {
        if (i >= _cameras.Count || i < 0) return;
        if (_cameras[i].gameObject.activeSelf)
        {
            DisableAll();
            PlayerLook.SetCameraEnabled(true);
        }
        else { DisableAll(); PlayerLook.SetCameraEnabled(false); _cameras[i].gameObject.SetActive(true); }
    }

    private void DisableAll()
    {
        foreach (var cam in _cameras) { cam.gameObject.SetActive(false); }
    }

    void Update()
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            if (_keys[i].wasPressedThisFrame)
            {
                SetCamera(i);
            }
        }
    }
}
