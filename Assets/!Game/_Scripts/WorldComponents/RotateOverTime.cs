using UnityEngine;
using UnityEngine.Audio;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Space space;
    [SerializeField] private AudioMixerUpdateMode mode = AudioMixerUpdateMode.Normal;
    private float deltaTime
    {
        get
        {
            if (mode == AudioMixerUpdateMode.Normal) return Time.deltaTime;
            else return Time.unscaledDeltaTime;
        }
    }

    private void Update()
    {
        transform.Rotate(rotation * deltaTime, space);
    }
}
