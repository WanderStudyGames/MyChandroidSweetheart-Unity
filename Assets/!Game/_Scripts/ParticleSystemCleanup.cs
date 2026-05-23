using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCleanup : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems;
    private void Start()
    {
        StartCoroutine(CleanupRoutine());
    }

    private IEnumerator CleanupRoutine()
    {
        bool cleanup = false;
        while (!cleanup)
        {
            cleanup = true;
            foreach (var particleSystem in particleSystems)
            {
                if (particleSystem.isPlaying) cleanup = false;
            }
            yield return null;
        }
        Destroy(gameObject);
    }

}
