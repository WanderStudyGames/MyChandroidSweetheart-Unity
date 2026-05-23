using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Companion))]
public abstract class CompanionComponent : CharacterComponent
{
    protected Companion companion;

    protected virtual void Awake()
    {
        companion = GetComponent<Companion>();
    }

}
