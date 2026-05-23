using System.Collections;

using UnityEngine;

public class SFXSource : MonoBehaviour
{
    [SerializeField] SFX sfx;
    public bool playOnAwake;
    public bool detachedPlaySource = false;
    [Tooltip("If true, the SFX will be silenced for a short time on start. Useful for puzzle elements with on-start effects.")] public bool timedSilence = false;
    public bool disableDoppler = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetOrAddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        if (disableDoppler)
            audioSource.dopplerLevel = 0;
    }
    private IEnumerator Start()
    {
        if (timedSilence)
        {
            enabled = false;
            yield return new WaitForSeconds(1f);
            enabled = true;
            yield break;
        }
        yield return null;
        if (playOnAwake && enabled)
        {
            if (!sfx.Loop) Play();
            else Play(0.3f);
        }
    }
    public void SetSFX(SFX sFX) { sfx = sFX; }
    public void Play()
    {
        Play(sfx);
    }

    public void Play(SFX sfxLocal)
    {
        if (!enabled) return;
        if (sfxLocal == null) return;
        if (detachedPlaySource)
        {
            audioSource = sfxLocal.PlayAtPoint(transform.position);
            return;
        }
        if (!gameObject.activeInHierarchy) return;
        if (audioSource == null)
            audioSource = gameObject.GetOrAddComponent<AudioSource>();
        audioSource.PlaySFX(sfxLocal);
    }

    public void Play(float fadeTime)
    {
        Play();
        audioSource.volume = 0;
        StopAllCoroutines();
        StartCoroutine(ExtensionMethods.Co_FadeFloat(fadeTime, new(0, sfx.Volume), fl =>
        {
            audioSource.volume = fl;
        }));

    }
    public void Stop()
    {
        if (sfx == null) return;
        if (audioSource == null) return;
        Debug.LogWarning("Stopping SFX: " + sfx.name, this);
        audioSource.Stop();
    }
    public void Stop(float fadeTime)
    {
        if (sfx == null) return;
        if (audioSource == null) return;

        StopAllCoroutines();
        StartCoroutine(Co_Stop());
        IEnumerator Co_Stop()
        {
            yield return ExtensionMethods.Co_FadeFloat(fadeTime, new(audioSource.volume, 0), fl =>
            {
                audioSource.volume = fl;
            });
            audioSource.Stop();
        }

    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if (!detachedPlaySource && audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}
