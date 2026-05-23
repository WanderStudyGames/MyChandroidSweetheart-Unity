using UnityEngine;

public class SetRotation : MonoBehaviour
{
    public void SetYRotation(float f)
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, f, transform.localRotation.eulerAngles.z);
    }
}
