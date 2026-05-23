using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIComponent : ObjectComponent
{
    protected UIRoot _uiRoot;

    protected virtual void Awake()
    {
        _uiRoot = GetComponentInParent<UIRoot>();
        if (_uiRoot == null) { Destroy(gameObject); Logger.Error(name + ": UI Root was not found in parent."); return; }
    }

}
