using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicFader : MonoBehaviour
{
    private bool _waitingForStop;
    public BGM BGM { get; private set; }
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    public void Play(BGM bgm)
    {
        _waitingForStop = false;
        BGM = bgm;
        audioSource.clip = bgm.track;
        StopAllCoroutines();
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeIn(3f));
        }
        else
        {
            audioSource.volume = 0;
            StartCoroutine(Co_Play());
        }

        IEnumerator Co_Play()
        {
            yield return new WaitForSecondsRT(BGM.startDelay);
            yield return null;
            audioSource.Play();
            StopAllCoroutines();
            StartCoroutine(FadeIn(BGM.fadeTime.x));
        }
    }

    public IEnumerator FadeIn(float fadeTime)
    {
        yield return ExtensionMethods.Co_FadeFloat(fadeTime, new(audioSource.volume, BGM.volume), fl =>
        {
            audioSource.volume = fl;
        }, true);
    }
    public void Stop()
    {
        if (_waitingForStop) { return; }
        StopAllCoroutines();
        if (!audioSource.isPlaying) return;

        StartCoroutine(Co_Stop());
        IEnumerator Co_Stop()
        {
            yield return ExtensionMethods.Co_FadeFloat(BGM.fadeTime.y, new(audioSource.volume, 0), fl =>
            {
                audioSource.volume = fl;
            }, true);
            _waitingForStop = true;
            Debug.Log("killing " + BGM.name + " in 10 seconds...");
            yield return new WaitForSeconds(BGM.timeToResume);
            Debug.Log("killed " + BGM.name + ".");
            audioSource.Stop();
            _waitingForStop = false;
        }
    }
}
