using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class SprintIndicator : MonoBehaviour
{
    [SerializeField] private GameObject on;
    [SerializeField] private GameObject off;
    [SerializeField] private SFX sfx;
    private AudioSource audioSource;
    private static bool sprinting = true;
    private static CanvasGroup sprintingGroup;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        sprintingGroup = GetComponent<CanvasGroup>();
        sprintingGroup.alpha = 0;
        Set(sprinting);
    }
    private void OnEnable()
    {
        PlayerMove.OnSprint += Sprint;
        PlayerMove.OnWalk += Walk;
    }
    private void OnDisable()
    {
        PlayerMove.OnSprint -= Sprint;
        PlayerMove.OnWalk -= Walk;
    }
    private void Audio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlaySFX(sfx);
        }
    }
    private void Set(bool b)
    {
        on.SetActive(b); off.SetActive(!b);
    }
    private void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(Co_FadeOut());
    }
    private IEnumerator Co_FadeOut()
    {
        sprintingGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);
        StartCoroutine(ExtensionMethods.Co_FadeFloat(2f, new(1, 0), (float f) => { sprintingGroup.alpha = f; }));
    }
    private void Sprint()
    {
        if (!sprinting)
        {
            Audio();
            Set(true);
            sprinting = true;
            FadeOut();
        }
    }
    private void Walk()
    {
        if (sprinting)
        {
            Audio();
            Set(false);
            sprinting = false;
            FadeOut();
        }
    }

}
