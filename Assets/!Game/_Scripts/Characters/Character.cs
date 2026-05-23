using System;
using UnityEngine;

public abstract class Character : EntityRoot
{
    public virtual void AttachTo(GameObject gameObject)
    {
        transform.SetParent(gameObject.transform, true);
    }
    public Action<GameObject> OnAttach;
    public Action OnDetach;
    public virtual void Detach() { transform.SetParent(null, true); transform.localScale = Vector3.one; }
}
