using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public static class ExtensionMethods
{
    /// <summary>
    /// Adds item to list only if it's not already contained in the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns>true if new item was added</returns>
    public static bool AddUnique<T>(this List<T> list, T item)
    {
        if (list == null) return false;
        if (item == null) return false;
        if (list.Contains(item)) return false;
        list.Add(item);
        return true;
    }
    /// <summary>
    /// Removes all instances of item from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns>true if instance was removed</returns>
    public static bool RemoveAll<T>(this List<T> list, T item)
    {
        if (list == null) return false;
        if (list.Count == 0) return false;
        bool found = false;
        while (list.Contains(item))
        {
            if (list.Remove(item))
                found = true;
        }
        return found;

    }
    public static Transform GetClosest(this Transform[] transforms, Vector3 pos)
    {
        Transform t = transforms[0];
        foreach (Transform t2 in transforms)
        {
            if (Vector3.Distance(t2.position, pos) < Vector3.Distance(t.position, pos))
                t = t2;
        }
        return t;
    }
    public static List<GameObject> SortListByDistanceFromPoint(this List<GameObject> l, Vector3 pos)
    {
        for (int i = l.Count - 1; i >= 0; i--)
        {
            if (!l[i].activeInHierarchy)
            {
                l.Remove(l[i]);
                i = l.Count - 1;
            }
            else if (i > 0 && Vector3.Distance(pos, l[i].transform.position) < Vector3.Distance(pos, l[i - 1].transform.position))
            {
                GameObject g = l[i];
                l[i] = l[i - 1];
                l[i - 1] = g;
                i = l.Count - 1;
            }
        }
        return l;
    }
    public static void Lerp(this Transform tr, Transform current, Transform target, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        tr.SetPositionAndRotation(
            Vector3.Lerp(current.position, target.position, t),
            Quaternion.Lerp(current.rotation, target.rotation, t)
            );
        tr.localScale = Vector3.Lerp(current.localScale, target.localScale, t);
    }
    public static IEnumerator Co_LerpOverTime(this Transform tr, Transform target, float durationInSeconds)
    {
        Vector3 pos = tr.position;
        Quaternion rot = tr.rotation;
        Vector3 scale = tr.localScale;

        float time = 0;
        while (time < durationInSeconds)
        {
            tr.Lerp(pos, rot, scale, target, time / durationInSeconds);
            time += Time.deltaTime;
            yield return null;
        }
        tr.Lerp(pos, rot, scale, target, 1);
    }
    public static void Lerp(this Transform tr, Vector3 startPos, Quaternion startRot, Vector3 startScale, Transform target, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        tr.SetPositionAndRotation(
            Vector3.Lerp(startPos, target.position, t),
            Quaternion.Lerp(startRot, target.rotation, t)
            );
        tr.localScale = Vector3.Lerp(startScale, target.localScale, t);
    }
    public static void ResetLocal(this Transform tr)
    {
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
        tr.localScale = Vector3.one;
    }
    public static void MoveTowards(this Transform tr, Transform current, Transform target, float maxDistanceDelta)
    {
        maxDistanceDelta = Mathf.Clamp(maxDistanceDelta, 0, 1);
        tr.SetPositionAndRotation(
            Vector3.MoveTowards(current.position, target.position, maxDistanceDelta),
            Quaternion.RotateTowards(current.rotation, target.rotation, maxDistanceDelta)
            );
        tr.localScale = Vector3.MoveTowards(current.localScale, target.localScale, maxDistanceDelta);
    }
    public static void MoveTowards(this Transform tr, Transform current, Transform target, float maxDistanceDelta, float maxRotationDelta)
    {
        maxDistanceDelta = Mathf.Clamp(maxDistanceDelta, 0, 1);
        tr.SetPositionAndRotation(
            Vector3.MoveTowards(current.position, target.position, maxDistanceDelta),
            Quaternion.RotateTowards(current.rotation, target.rotation, maxRotationDelta)
            );
        tr.localScale = Vector3.MoveTowards(current.localScale, target.localScale, maxDistanceDelta);
    }

    public static void SetStartColor(this ParticleSystem ps, Color c)
    {
        if (ps == null) { Logger.Error($"Null reference exception for {nameof(ps)}"); }
        ParticleSystem.MainModule main = ps.main;
        main.startColor = c;
    }
    public static void SetAndPlay(this AudioSource aus, AudioClip clip, float volume, bool loop, float spatialBlend, AudioMixerGroup mixerGroup, bool bypassReverb, Vector2 pitchVariation)
    {
        aus.clip = clip;
        aus.volume = volume;
        aus.loop = loop;
        aus.spatialBlend = spatialBlend;
        aus.outputAudioMixerGroup = mixerGroup;
        aus.bypassReverbZones = bypassReverb;
        aus.pitch = 1 + UnityEngine.Random.Range(pitchVariation.x, pitchVariation.y);
        aus.Play();
    }

    public static void SetFromSFX(this AudioSource aus, SFX sfx, float volume = 100)
    {
        aus.clip = sfx.Clip;
        if (volume == 100) aus.volume = sfx.Volume;
        else aus.volume = volume;
        aus.loop = sfx.Loop;
        aus.spatialBlend = sfx.SpatialBlend;
        aus.outputAudioMixerGroup = sfx.MixerGroup;
        aus.bypassReverbZones = sfx.BypassReverb;
        aus.spread = sfx.Spread;
        aus.pitch = 1 + UnityEngine.Random.Range(sfx.PitchVariation.x, sfx.PitchVariation.y);
    }

    public static void PlaySFX(this AudioSource aus, SFX sfx, float volume = 100)
    {
        if (!aus.isActiveAndEnabled) return;
        aus.clip = sfx.Clip;
        if (volume == 100) aus.volume = sfx.Volume;
        else aus.volume = volume;
        aus.loop = sfx.Loop;
        aus.spatialBlend = sfx.SpatialBlend;
        aus.outputAudioMixerGroup = sfx.MixerGroup;
        aus.bypassReverbZones = sfx.BypassReverb;
        aus.spread = sfx.Spread;
        aus.pitch = 1 + UnityEngine.Random.Range(sfx.PitchVariation.x, sfx.PitchVariation.y);
        if (!aus.isActiveAndEnabled) Debug.LogError(aus.gameObject.name, aus.gameObject);
        aus.Play();
    }

    //public static IEnumerator PlaySFXRoutine(this AudioSource au, SFX sfx, float fadeInTime = 0, float delayTime = 0)
    //{
    //    if (sfx == null || sfx.Clip == null) return null;
    //    return (au.PlayFadeInRoutine(fadeInTime, delayTime, sfx));
    //}




    public static IEnumerator Co_FadeFloat(float fadeTime, Vector2 valuesBetween, Action<float> action, bool unscaledTime = false)
    {
        float fl;
        float time = 0;
        while (time < fadeTime)
        {
            fl = Mathf.Lerp(valuesBetween.x, valuesBetween.y, time / fadeTime);
            action(fl);
            if (unscaledTime) time += Time.unscaledDeltaTime;
            else time += Time.deltaTime;
            yield return null;
        }
        action(valuesBetween.y);
    }

    public static IEnumerator StopRoutine(this AudioSource au, float fadeOutTime = 0, float delayTime = 0, bool destroy = false)
    {
        float startVolume = au.volume;
        float time = 0;
        yield return new WaitForSeconds(delayTime);
        while (time < fadeOutTime)
        {
            au.volume = (-(startVolume / fadeOutTime) * time) + startVolume;
            time += Time.deltaTime;
            yield return null;
        }
        au.volume = 0;
        au.Stop();
        if (destroy) { GameObject.Destroy(au.gameObject); }
    }


    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        if (!go.TryGetComponent(out T component))
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static bool Contains(this LayerMask lm, int layer)
    {
        return lm == (lm | (1 << layer));
    }
    public static LayerMask Exclude(this LayerMask lm, int excludedLayer)
    {
        return lm & ~(1 << excludedLayer);
    }
    public static Vector2 XZ(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }
    public static Vector2 Vector2ToWorldSpace(this Transform tr, Vector2 vector2)
    {
        Vector2 controlVelocityTransformed;

        Vector3 vector3 = new(vector2.x, 0, vector2.y);

        vector3 = tr.TransformVector(vector3);
        controlVelocityTransformed = new(vector3.x, vector3.z);

        return controlVelocityTransformed;
    }
    /// <summary>
    /// Given a string representing a path in forward-slash format, returns the path of the parent folder.
    /// Given "Assets/Textures/tex.png", returns "Assets/Textures".
    /// </summary>
    /// <param name="path"></param>
    /// <returns>the path of the parent folder</returns>
    public static string AscendDir(this string path)
    {
        if (!path.Contains('/')) return path;
        return path[..path.LastIndexOf("/")];
    }
    public static string SwapDir(this string path, string newDirectory)
    {
        var item = path[path.LastIndexOf("/")..];
        return newDirectory + item;
    }
    public static string DirItem(this string path)
    {
        return path[(path.LastIndexOf("/") + 1)..];
    }
    public static void ReassignArmature(this SkinnedMeshRenderer smr, SkinnedMeshRenderer newParent)
    {
        smr.rootBone = newParent.rootBone;
        smr.bones = newParent.bones;
    }

    public static void SetActive(this List<GameObject> list, bool enabled)
    {
        foreach (GameObject go in list)
        {
            if (go != null)
                go.SetActive(enabled);
        }
    }

    public static void SetMaterials(this GameObject gameObject, Material material)
    {
        MeshRenderer mRenderer = gameObject.GetComponent<MeshRenderer>();
        SkinnedMeshRenderer sMRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        MeshFilter mFilter = gameObject.GetComponent<MeshFilter>();
        if (mRenderer != null && mFilter != null)
        {
            List<Material> mats = new();
            for (int i = 0; i < mRenderer.materials.Length; i++) { mats.Add(material); }
            mRenderer.materials = mats.ToArray();
        }
        else if (!sMRenderer.IsUnityNull())
        {
            List<Material> mats = new();
            for (int i = 0; i < sMRenderer.materials.Length; i++) { mats.Add(material); }
            sMRenderer.materials = mats.ToArray();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>Closest Cardinal direction to provided Vector3 parameter</returns>
    public static Vector3 GetQuantizedDirection(this Vector3 direction, Vector3[] directions = null)
    {
        if (directions == null) { directions = new Vector3[] { Vector3.right, Vector3.left, Vector3.down, Vector3.up, Vector3.forward, Vector3.back }; }
        Vector3 best = directions[0];
        for (int i = 1; i < directions.Length; i++)
        {
            if (Vector3.Dot(direction.normalized, best) < Vector3.Dot(direction.normalized, directions[i]))
            {
                best = directions[i];
            }
        }
        return best;
    }
    public static void SetValues(this NavMeshAgent NMAgent, NMAProfile navMeshAgentProfile)
    {
        if (navMeshAgentProfile == null) return;
        NMAgent.baseOffset = navMeshAgentProfile.baseOffset;
        NMAgent.speed = navMeshAgentProfile.speed;
        NMAgent.angularSpeed = navMeshAgentProfile.angularSpeed;
        NMAgent.acceleration = navMeshAgentProfile.acceleration;
        NMAgent.stoppingDistance = navMeshAgentProfile.stoppingDistance;
        NMAgent.autoBraking = navMeshAgentProfile.autoBraking;
    }
    public static bool QuantizeToNavmesh(this Vector3 vector3, out Vector3 quantizedVector3)
    {
        quantizedVector3 = vector3;
        if (NavMesh.SamplePosition(vector3, out NavMeshHit hit, 100f, NavMesh.AllAreas))
        {
            quantizedVector3 = hit.position;
            return true;
        }
        return false;
    }
    public static bool SetClosestReachableDestination(this NavMeshAgent NMAgent, Vector3 destination)
    {
        var path = new NavMeshPath();
        if (!NMAgent.CalculatePath(destination, path))
        {
            if (destination.QuantizeToNavmesh(out Vector3 quantized))
            {

                return SetClosestReachableDestination(NMAgent, quantized);
            }
        }
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            NMAgent.SetDestination(path.corners.Last());
        }
        else { NMAgent.SetDestination(destination); }
        return true;
    }
    public static bool CheckIsStopped(this NavMeshAgent nma)
    {
        return !nma.isOnNavMesh || !nma.isActiveAndEnabled || nma.isStopped;
    }

    public static IEnumerator Co_EnableWhenNavMeshExists(this NavMeshAgent nma)
    {
        while (!nma.NavMeshExists())
        {
            yield return null;
        }
        nma.enabled = true;
    }
    public static bool NavMeshExists(this NavMeshAgent nma)
    {
        return NavMesh.SamplePosition(nma.gameObject.transform.position, out _, 10f, NavMesh.AllAreas);
    }
    public static void Link(this InputAction inputAction, Action<InputAction.CallbackContext> action)
    {
        inputAction.started += action;
        inputAction.performed += action;
        inputAction.canceled += action;
    }
    public static void UnLink(this InputAction inputAction, Action<InputAction.CallbackContext> action)
    {
        inputAction.started -= action;
        inputAction.performed -= action;
        inputAction.canceled -= action;
    }
    public static void Link(this InputActionAsset inputActions, string actionName, Action<InputAction.CallbackContext> action)
    {
        inputActions[actionName].started += action;
        inputActions[actionName].performed += action;
        inputActions[actionName].canceled += action;
    }
    public static void UnLink(this InputActionAsset inputActions, string actionName, Action<InputAction.CallbackContext> action)
    {
        inputActions[actionName].started -= action;
        inputActions[actionName].performed -= action;
        inputActions[actionName].canceled -= action;
    }
    public static void SetMaps(this InputActionAsset inputActions, string[] mapNames)
    {
        foreach (string name in mapNames)
        {
            InputActionMap map = inputActions.FindActionMap(name);

            if (!map.enabled)
            {
                map.Enable();
            }
        }
        List<InputActionMap> mapsToDisable = new();
        foreach (InputAction action in inputActions)
        {
            if (!mapNames.Contains(action.actionMap.name) && !mapsToDisable.Contains(action.actionMap))
            {
                mapsToDisable.Add(action.actionMap);
            }
        }
        foreach (InputActionMap map in mapsToDisable)
        {
            map.Disable();
        }
        //Debug.Log($"Set input map to {mapNames[0]}");

    }
    public static void DebugLog(this InputAction.CallbackContext ctx)
    {
        if (ctx.started) Debug.Log($"started, {ctx.action.activeControl.name}, {ctx.action.ReadValue<float>()}");
        if (ctx.performed) Debug.Log($"performed, {ctx.action.activeControl.name}, {ctx.action.ReadValue<float>()}");
        if (ctx.canceled) Debug.Log($"canceled, {ctx.action.activeControl.name}, {ctx.action.ReadValue<float>()}");
        if (ctx.action.WasPressedThisFrame()) Debug.Log($"pressed, {ctx.action.activeControl.name}, {ctx.action.ReadValue<float>()}");
        if (ctx.action.WasReleasedThisFrame()) Debug.Log($"released, {ctx.action.activeControl.name}, {ctx.action.ReadValue<float>()}");
    }
    public static string GetScheme(this InputAction.CallbackContext ctx)
    {
        return ctx.action.GetBindingForControl(ctx.control).Value.groups.Split(";")[0];
    }
    public static string Capitalize(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        if (s.Length == 1) return s.ToUpper();

        return char.ToUpper(s[0]) + s.Substring(1);
    }
    public static string ReplaceAll(this string s, string searchString, string replacement)
    {
        if (searchString == replacement) return s;
        while (s.IndexOf(searchString) > -1)
        {
            s = s.Replace(searchString, replacement);
        }
        return s;
    }
    public static string RemoveTags(this string str)
    {
        string s = str;
        int start = s.IndexOf("<");
        int end = -1;
        if (start < s.Length && start > -1)
            end = s.IndexOf(">", start);
        while (start > -1 && end > -1)
        {
            s = s.Remove(start, end + 1 - start);
            start = s.IndexOf("<");
            if (start < s.Length && start > -1)
                end = s.IndexOf(">", start);
        }
        return s;

    }
    public static void SetEnabled(this InputActionAsset iaa, string[] actions, bool enabled)
    {
        foreach (string action in actions)
        {
            if (enabled) iaa[action].Enable();
            else iaa[action].Disable();
        }
    }

    public static bool FallsBetween(this float f, Vector2 values)
    {
        if (values.x < values.y)
            return f > values.x && f < values.y;
        else { return f < values.x && f > values.y; }
    }
    public static void SetToonColor(this Material material, Color color)
    {
        var baseColorID = Shader.PropertyToID("_BaseColor");
        var shadowColorID = Shader.PropertyToID("_1st_ShadeColor");
        var deepShadowColorID = Shader.PropertyToID("_2nd_ShadeColor");
        material.SetColor(baseColorID, color);
        Color.RGBToHSV(color, out float h, out float s, out float v);
        material.SetColor(shadowColorID, Color.HSVToRGB(h, s * 1.1f, v * 0.8f));
        //var deepShadowColor = new Color(shadowColor.r * 0.8f, shadowColor.g * 0.8f, shadowColor.b * 0.85f);
        material.SetColor(deepShadowColorID, Color.HSVToRGB(h, s * 1.3f, v * 0.6f));
    }
    public static void SetToonColors(this Material material, Color color, Color shadowColor, Color deepShadowColor)
    {
        var baseColorID = Shader.PropertyToID("_BaseColor");
        var shadowColorID = Shader.PropertyToID("_1st_ShadeColor");
        var deepShadowColorID = Shader.PropertyToID("_2nd_ShadeColor");
        material.SetColor(baseColorID, color);
        material.SetColor(shadowColorID, shadowColor);
        material.SetColor(deepShadowColorID, deepShadowColor);
    }
    public static List<string> NamesFromAssets<T>(List<T> clothings) where T : ScriptableObject
    {
        List<string> strings = new();
        foreach (var clothing in clothings)
        {
            if (clothing == null) continue;
            strings.Add(clothing.name);
        }
        return strings;
    }
    public static List<T> AssetsFromNames<T>(List<string> names) where T : ScriptableObject
    {
        List<T> clothings = new();
        foreach (var str in names)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var cl = Resources.Load<T>(str);
                if (cl != null) clothings.Add(cl);
            }
        }
        return clothings;
    }
    public static T[] GetComponentsFromImmediateChildren<T>(this GameObject gameObject, bool searchInactive = false) where T : Component
    {
        var transform = gameObject.transform;
        List<T> components = new();
        for (int i = 0; i < transform.childCount; i++)
        {

            Transform child = transform.GetChild(i);
            if (!searchInactive && !child.gameObject.activeInHierarchy) continue;
            T component = child.GetComponent<T>();
            if (component != null)
            {
                components.Add(component);
            }
        }
        return components.ToArray();
    }
    /// <summary>
    /// Performs the provided action, which should include clip replacement in an animator override controller.
    /// <br></br>
    /// Requires empty "Transition" state in AnimatorController, with write defaults disabled.
    /// <br></br>
    /// <br></br>
    /// Note: Will pause current animation during transition.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="performClipReplacement"></param>
    public static void ClipReplacementCrossFade(this Animator animator, float transitionDuration, Action performClipReplacement, string[] statesWhitelist = null)
    {
        var s = animator.GetCurrentAnimatorStateInfo(0);
        var hash = s.shortNameHash;
        var time = s.normalizedTime;
        var exit = false;

        if (statesWhitelist != null)
        {
            exit = true;
            foreach (var state in statesWhitelist)
            {
                if (s.shortNameHash == Animator.StringToHash(state)) exit = false;
            }
        }

        if (exit || s.shortNameHash == Animator.StringToHash("Transition"))
        {
            performClipReplacement?.Invoke();
            return;
        }
        performClipReplacement?.Invoke();
        animator.Play("Transition", 0, time);
        animator.Update(0);
        animator.CrossFade(s.shortNameHash, transitionDuration, 0, time);
    }
    public static void ResetAllTriggers(this Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
    public static void PhysicsFriendlyDisable(this MonoBehaviour mb, bool destroy = false, Action onDisable = null)
    {
        var oldPosition = mb.transform.position;
        mb.transform.position = Vector3.one * -100f;
        Physics.SyncTransforms();
        mb.StartCoroutine(Co_Disable());
        IEnumerator Co_Disable()
        {
            yield return new WaitForFixedUpdate();
            yield return null;
            mb.gameObject.SetActive(false);
            if (destroy) GameObject.Destroy(mb.gameObject);
            else mb.transform.position = oldPosition;
            onDisable?.Invoke();
        }
    }
}
