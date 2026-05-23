using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class AttachablePlatform : MonoBehaviour
{
    [SerializeField] private bool _moveable = true;
    private Player _player;
    private List<IAttachableCharacter> _overlappingNPCs = new();
    private List<IAttachableCharacter> _attachedNPCs = new();

    public void TeleportNPCsToClosest(Transform[] transforms)
    {
        foreach (var npc in _overlappingNPCs)
        {
            npc.TeleportToClosest(transforms);
        }
    }
    public void AttachNPCS()
    {
        if (!_moveable) return;
        foreach (var npc in _overlappingNPCs) { npc.AttachTo(gameObject); _attachedNPCs.Add(npc); }
    }
    public void DetachNPCS()
    {
        foreach (var npc in _overlappingNPCs) { if (_attachedNPCs.Contains(npc)) npc.Detach(); }
    }
    public bool Contains(IAttachableCharacter attachableCharacter)
    {
        return _overlappingNPCs.Contains(attachableCharacter);
    }
    private void OnValidate()
    {
        //if (GetComponent<Renderer>() != null)
        //{
        //    Debug.LogWarning("Mesh renderer detected on " + gameObject.name + ". The best way to use the " + nameof(AttachablePlatform) + " component is to create an empty child with its own collider defining the attachment area above the platform!", gameObject);
        //}
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag) && _moveable)
        {
            _player = other.gameObject.GetComponent<Player>();
            _player.AttachTo(gameObject);
        }
        else if (other.gameObject.TryGetComponent(out IAttachableCharacter npc))
        {
            _overlappingNPCs.AddUnique(npc);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag))
        {
            if (_player != null) _player.Detach();
            _player = null;
        }
        else if (other.gameObject.TryGetComponent(out IAttachableCharacter npc))
        {
            if (_attachedNPCs.Contains(npc))
            {
                npc.Detach();
                _attachedNPCs.RemoveAll(npc);
            }
            _overlappingNPCs.RemoveAll(npc);
        }
    }
}