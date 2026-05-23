using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractibleObject : MonoBehaviour
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool interactOnce;
    [SerializeField] private UnityEvent OnInteract;
#if UNITY_EDITOR
    [SerializeField] private UnityEvent DebugActions;
#endif
    [SerializeField] private bool playAudio;
    [field: SerializeField] public Sprite Icon { get; set; }
    private List<string> _embargos = new();
    private bool _ignoreEmbargos = false;
    public void IgnoreEmbargos(bool ignore)
    {
        _ignoreEmbargos = ignore;
    }
    public void AddEmbargo(string embargo)
    {
        _embargos.AddUnique(embargo);
    }
    public void RemoveEmbargo(string embargo)
    {
        _embargos.RemoveAll(embargo);
    }
    public bool CanInteract()
    {
        if (_ignoreEmbargos) return canInteract;
        else return canInteract && _embargos.Count == 0;
    }
    public void SetCanInteract(bool b) { canInteract = b; }
    public void Interact()
    {
        if (CanInteract())
        {
#if UNITY_EDITOR
            if (DebugActions.GetPersistentEventCount() > 0) DebugActions.Invoke();
            else
#endif
                OnInteract.Invoke();
            if (interactOnce) canInteract = false;
            if (playAudio)
            {
                if (TryGetComponent(out SFXSource sfxSource))
                {
                    sfxSource.Play();
                }
                else if (TryGetComponent(out AudioSource audioSource))
                {
                    audioSource.Play();
                }
            }
        }
    }
}
