using System.Collections;
using UnityEngine;

public class ShaderUnscaledTime : MonoBehaviour
{
    IEnumerator Start()
    {
        while (true)
        {
            Shader.SetGlobalFloat("_unscaledTime", Time.unscaledTime);
            yield return null;
        }
    }
}
