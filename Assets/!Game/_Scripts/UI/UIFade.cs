using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIFade : MonoBehaviour
{
    [Dependency][SerializeField] private GameObject _loadingText;
    public static Color FadeColor = Color.black;
    public static Vector3 FadeDurations = Vector3.one;
    private static GameObject loadingText;
    private static Image image;
    private static UIFade instance;
    public static bool Exists => instance != null;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Load() { Init(); PlayMode.OnEnterPlayMode += Init; }
    private static void Init() { FadeColor = Color.black; FadeDurations = Vector3.one * 0.3f; }
    private void Awake()
    {
        loadingText = _loadingText;
        instance = this;
        image = GetComponent<Image>();
    }
    private void Start()
    {
    }
    private void OnEnable()
    {
        FadeIn();
        //image.color = new(image.color.r, image.color.g, image.color.b, 0);
    }
    public static void ExecuteAfterFade(Action afterFadeAction, bool showLoadingtext = true)
    {
        instance.StartCoroutine(Co_ExecuteAfterFade());
        IEnumerator Co_ExecuteAfterFade()
        {
            yield return ExtensionMethods.Co_FadeFloat(FadeDurations.x, new(image.color.a, 1), (float fl) =>
            {
                image.color = new(FadeColor.r, FadeColor.g, FadeColor.b, fl);
            });
            afterFadeAction?.Invoke();
            loadingText.SetActive(showLoadingtext);
        }
    }

    public static void LoadingEnabled(bool b)
    {
        loadingText.SetActive(b);
    }

    public static void FadeIn()
    {
        if (instance == null || !instance.gameObject.activeInHierarchy) return;
        image.color = new(FadeColor.r, FadeColor.g, FadeColor.b, 1);
        if (FadeDurations.z > 0)
        {
            instance.StartCoroutine(Co_FadeIn());
        }
        else
        {
            image.color = new(FadeColor.r, FadeColor.g, FadeColor.b, 0);
        }



        IEnumerator Co_FadeIn()
        {
            yield return null;
            yield return new WaitForSeconds(FadeDurations.y);
            yield return ExtensionMethods.Co_FadeFloat(FadeDurations.z, new(image.color.a, 0), (float fl) =>
            {
                image.color = new(FadeColor.r, FadeColor.g, FadeColor.b, fl);
            });
        }
    }
}
