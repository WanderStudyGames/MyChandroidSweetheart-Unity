using UnityEngine;

public class SetUniformScale : MonoBehaviour
{
    public void SetScale(float f)
    {
        transform.localScale = Vector3.one * f;
    }
}
