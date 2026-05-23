using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetFog : MonoBehaviour
{
    [SerializeField] private float _durationInSeconds;
    public void SetDensity(float density)
    {
        StopAllCoroutines();
        StartCoroutine(Co_FogDensity(density, _durationInSeconds));
    }
    public void SetDensity(float density, float durationInSeconds)
    {
        StopAllCoroutines();
        StartCoroutine(Co_FogDensity(density, durationInSeconds));
    }
    private IEnumerator Co_FogDensity(float density, float duration = 0)
    {
        float time = 0;
        float startDensity = RenderSettings.fogDensity;
        while(time < duration)
        {
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, density, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogDensity = density;
    }
}
