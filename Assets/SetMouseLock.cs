using UnityEngine;

public class SetMouseLock : MonoBehaviour
{
    [SerializeField] private CursorLockMode m_CursorLockMode = CursorLockMode.Locked;
    private void OnEnable()
    {
        Cursor.lockState = m_CursorLockMode;
    }
}
