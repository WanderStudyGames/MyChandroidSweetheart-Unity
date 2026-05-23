using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcProfileController : MonoBehaviour
{
    private static PostProcProfileController instance;
    public static void SetProfile(PostProcessProfile postProcessProfile) { instance.ppv.profile = postProcessProfile; }
    public static void ResetProfile() { if (instance == null) return; instance.ppv.profile = instance._defaultProfile; }
    private PostProcessVolume ppv;
    private PostProcessProfile _defaultProfile;
    [SerializeField] private PostProcessVolume _sceneTransition;
    private static float _sceneTransitionWeight;
    private static Vector3 _sceneTransitionTimes = Vector3.one;
    void Awake()
    {

        ppv = GetComponent<PostProcessVolume>();
        _defaultProfile = ppv.sharedProfile;
    }
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }
    private void Start()
    {
        if (_sceneTransitionWeight > 0)
        {
            FadeIn();
            Debug.Log(_sceneTransitionTimes);
        }
    }
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    private static void Enable()
    {
        PlayMode.OnEnterPlayMode += Init;
    }
    private static void Init() { _sceneTransitionWeight = 0; if (instance != null) instance._sceneTransition.weight = 0; }
    private void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(Co_FadeIn());
        IEnumerator Co_FadeIn()
        {
            yield return new WaitForSeconds(_sceneTransitionTimes.y);
            yield return ExtensionMethods.Co_FadeFloat(_sceneTransitionTimes.z, new(_sceneTransitionWeight, 0), fl => { _sceneTransition.weight = fl; _sceneTransitionWeight = fl; });
        }
    }
    public static void ExecuteAfterTransition(Action afterTransitionAction, PostProcessProfile transitionProfile = null, Vector3 transitionTimes = default)
    {
        if (instance == null)
        {
            Debug.LogWarning("No PostProcProfileController in scene, cannot do transition.");
            afterTransitionAction?.Invoke();
            return;
        }
        _sceneTransitionTimes = transitionTimes;
        if (transitionProfile != null) instance._sceneTransition.profile = transitionProfile;
        instance.StartCoroutine(ExtensionMethods.Co_FadeFloat(_sceneTransitionTimes.x, new(_sceneTransitionWeight, 1), (float fl) =>
        {
            instance._sceneTransition.weight = fl;
            _sceneTransitionWeight = fl;
            if (fl == 1)
            {
                //loadingText.SetActive(showLoadingtext);
                afterTransitionAction?.Invoke();
                instance.FadeIn();
            }
        }));
    }
}
