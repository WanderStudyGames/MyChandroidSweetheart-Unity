using System.Collections;

using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class SceneLoader : MonoBehaviour
{

    [field: SerializeField] private SceneAssetReference _sceneAssetReference;
    public SceneAssetReference SceneAssetReference => _sceneAssetReference;
    [FormerlySerializedAs("ignoreSpawnpoint")]
    [SerializeField] protected bool _ignoreSpawnpoint;
    [FormerlySerializedAs("spawnpointToLoad")]
    [SerializeField] protected int _spawnpointToLoad;
    [FormerlySerializedAs("fadeColor")]
    [SerializeField] private Color _fadeColor = Color.black;
    public Color FadeColor => _fadeColor;
    [FormerlySerializedAs("fadeTime")]
    [SerializeField] protected Vector3 _fadeTime = Vector3.one * 0.3f;

    [SerializeField] private SFX _walkStartSFX;
    [SerializeField] private SFX _walkEndSFX;
    private bool _walking;
    public virtual void LoadScene()
    {

        if (UIFade.Exists)
        {
            if (_walkStartSFX != null && _walkEndSFX != null)
                StartCoroutine(Co_Walk());
            UIFade.FadeColor = _fadeColor;

            UIFade.FadeDurations = _fadeTime;
            UIFade.ExecuteAfterFade(() =>
            {
                Load();
            });
        }
        else Load();

        IEnumerator Co_Walk()
        {
            _walking = true;
            _walkStartSFX.PlayAtPoint(PlayerData.Position).volume = 0.12f;
            yield return new WaitForSeconds(0.3f);
            _walkStartSFX.PlayAtPoint(PlayerData.Position).volume = 0.12f;
            yield return new WaitForSeconds(0.3f);
            _walkEndSFX.PlayAtPoint(PlayerData.Position).volume = 0.12f;
            yield return new WaitForSeconds(0.3f);
            _walkEndSFX.PlayAtPoint(PlayerData.Position).volume = 0.12f;
            yield return new WaitForSeconds(0.3f);
            _walking = false;
        }
    }

    private void Load()
    {
        StartCoroutine(Co_Load());

        IEnumerator Co_Load()
        {
            yield return new WaitUntil(() => !_walking);
            if (SceneAssetReference.SceneName == "") { Logger.Error("Scene asset not assigned", context: this.gameObject); yield break; }
            SceneHandler.LoadScene(SceneAssetReference.SceneName, (_ignoreSpawnpoint) ? -1 : _spawnpointToLoad);

        }
    }

#if UNITY_EDITOR
    public static SceneAssetReference PreviousScene { get; set; }
    private static void FocusOn(SceneAssetReference sceneAsset)
    {
        var objects = FindObjectsOfType<SceneLoader>();
        foreach (SceneLoader sl in objects)
        {
            if (sl.SceneAssetReference.SceneName == sceneAsset.SceneName)
            {
                Selection.activeGameObject = sl.gameObject;
                Selection.activeObject = sl.gameObject;
                SceneView.lastActiveSceneView.pivot = sl.transform.position;
                break;
            }
        }
    }
    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        if (PreviousScene != null)
        {
            FocusOn(PreviousScene);
            PreviousScene = null;
        }
    }
#endif

}