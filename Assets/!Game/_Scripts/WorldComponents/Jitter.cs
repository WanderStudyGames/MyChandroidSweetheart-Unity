using System;
using System.Collections;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private float rotationAmount;
    [SerializeField] private bool startOnAwake = false;
    [Range(0, 1)] public float mix = 1;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation.eulerAngles;
    }
    private void OnEnable()
    {
        if (startOnAwake) StartJitter(mix);
    }
    public void StartJitterTimer(float time)
    {
        StartJitterTimer(time / 3, time / 3, time / 3);
    }
    public void StartJitterTimer(float fadeInTime, float holdTime, float fadeOutTime)
    {
        StartJitter(0.001f);
        StartCoroutine(Co_StartJitterTimer());
        IEnumerator Co_StartJitterTimer()
        {
            yield return ExtensionMethods.Co_FadeFloat(fadeInTime, Vector2.up, fl =>
            {
                mix = fl;
            });
            yield return new WaitForSeconds(holdTime);
            yield return ExtensionMethods.Co_FadeFloat(fadeOutTime, Vector2.right, fl =>
            {
                mix = fl;
            });
            mix = 0;
        }
    }
    /// <summary>
    /// Starts jitter coroutine which lasts until mix is 0
    /// </summary>
    public void StartJitter(float mix)
    {
        this.mix = mix;
        StopAllCoroutines();
        StartCoroutine(Co_MixControlledJitter());
        IEnumerator Co_MixControlledJitter()
        {
            while (true)
            {
                Randomize();
                yield return null;
            }
            //Randomize();
        }
    }
    private void Randomize()
    {
        if (amount > 0)
        {
            transform.localPosition = new(
                initialPosition.x + (UnityEngine.Random.Range(-amount, amount) * mix),
                initialPosition.y + (UnityEngine.Random.Range(-amount, amount) * mix),
                initialPosition.z + (UnityEngine.Random.Range(-amount, amount) * mix)
                );
        }
        if (rotationAmount > 0)
        {
            transform.localRotation = Quaternion.Euler(
            initialRotation.x + (UnityEngine.Random.Range(-rotationAmount, rotationAmount) * mix),
            initialRotation.y + (UnityEngine.Random.Range(-rotationAmount, rotationAmount) * mix),
            initialRotation.z + (UnityEngine.Random.Range(-rotationAmount, rotationAmount) * mix));
        }

    }
}
