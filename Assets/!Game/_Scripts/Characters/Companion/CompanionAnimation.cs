using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class CompanionAnimation : MonoBehaviour
{
    Animator animator;
    [Dependency][SerializeField] private Rig LookAt;
    [Dependency][SerializeField] private CompanionManager CompanionManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = CompanionData.LocalVelocity;
        animator.SetFloat("Yvel", vel.y);
        animator.SetFloat("Xvel", vel.x);
        //LookAt.weight = 1 - Mathf.Clamp01(vel.magnitude);

    }
}
