using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GlobalUIClickSFX : MonoBehaviour
{
    [SerializeField] private SFX clickSFX;
    [SerializeField] private SFX unClickSFX;
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private bool checkFor3DUI = false;

    private Camera mainCamera;

    bool pointerDown = false;

    private void OnEnable()
    {
        if (checkFor3DUI && mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("No main camera found. 3D UI checks will not work properly.");
                checkFor3DUI = false;
            }
        }
        if (clickAction != null && clickAction.action != null)
        {
            clickAction.action.performed += OnClickPerformed;
        }
    }

    private void OnDisable()
    {
        if (clickAction != null && clickAction.action != null)
        {
            clickAction.action.performed -= OnClickPerformed;
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 pointerPos = Vector2.zero;

        if (ctx.control.device is Pointer pointer && pointer.added)
        {
            pointerPos = pointer.position.ReadValue();
        }
        else
        {
            if (Mouse.current != null)
                pointerPos = Mouse.current.position.ReadValue();
            else if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
                pointerPos = Touchscreen.current.touches[0].position.ReadValue();
        }

        var pressed = ctx.action.IsPressed();
        if (pressed && !pointerDown && TryPlaySFX(clickSFX, pointerPos))
        {
            pointerDown = true;
        }
        else if (!pressed && pointerDown)
        {
            unClickSFX?.PlayAtPoint(transform.position);
            pointerDown = false;
        }
    }

    private bool TryPlaySFX(SFX sfx, Vector2 screenPosition)
    {
        if (checkFor3DUI)
        {
            // Check if the click is on a 3D UI element
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Collider collider = hit.collider;
                if (collider != null)
                {
                    Debug.Log("Raycast hit: " + collider.gameObject.name);
                    if (IsOnIgnoredUI(collider.gameObject))
                        return false;
                }
            }
        }

        if (EventSystem.current == null)
        {
            sfx.PlayAtPoint(transform.position);
            return true;
        }
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = screenPosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Collider>() != null)
                Debug.Log("Raycast hit: " + result.gameObject.name);
            GameObject go = result.gameObject;
            if (IsOnIgnoredUI(go))
                return false;
        }

        if (sfx != null)
        {
            sfx.PlayAtPoint(transform.position);
            return true;
        }
        return false;
    }

    private bool IsOnIgnoredUI(GameObject go)
    {
        return go.GetComponentInParent<Button>() != null
            || go.GetComponentInParent<Toggle>() != null
            || go.GetComponentInParent<TabButtonComponent>() != null
            || go.GetComponentInParent<TMP_Dropdown>() != null
            || go.GetComponentInParent<Dropdown>() != null
            || go.GetComponent<ButtonAnimator>() != null
            ;
    }
}