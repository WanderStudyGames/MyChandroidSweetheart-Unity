using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComponentFlag
{
    private ComponentProfile _componentProfile;
    public virtual ComponentProfile Profile => _componentProfile;

    [SerializeField] protected bool _enabled;
    public virtual bool Enabled => _enabled;
    public void SetEnabled(bool b) { _enabled = b; }
    public ComponentFlag(bool enabled, ComponentProfile componentProfile)
    {
        _enabled = enabled;
        _componentProfile = componentProfile;
    }
}
