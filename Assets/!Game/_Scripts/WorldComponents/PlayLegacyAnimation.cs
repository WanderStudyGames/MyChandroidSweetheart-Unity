using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLegacyAnimation : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [ContextMenu("PlayAnim")]
    public void PlayAnim()
    {
        _animation.Play();
    }
}
