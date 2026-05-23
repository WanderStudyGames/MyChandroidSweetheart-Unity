using UnityEngine;

public interface IAttachableCharacter
{
    public void AttachTo(GameObject go);
    public void Detach();
    public void TeleportToClosest(Transform[] transforms);
}
