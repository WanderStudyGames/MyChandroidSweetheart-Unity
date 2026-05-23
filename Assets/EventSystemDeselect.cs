using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemDeselect : MonoBehaviour
{
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
