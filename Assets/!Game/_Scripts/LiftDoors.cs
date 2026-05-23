using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoors : MonoBehaviour
{
    [SerializeField] int floorNumber;
    [SerializeField] SFX openCloseSFX;
    [SerializeField] LerpOnCommand[] doors;

    private AudioSource audioSource;
    private bool unlocked = true;
    private bool colliding = false;
    private void Awake()
    {
        audioSource = gameObject.GetOrAddComponent<AudioSource>();
        if (floorNumber != 0)
        {
            unlocked = false;
            foreach (LerpOnCommand loc in doors)
            {
                loc.SetDestination(0);
            }
        }
        else if (unlocked)
        {
            foreach (LerpOnCommand loc in doors)
            {
                loc.SetDestination(0);
            }
        }
    }
    public void CheckUnlock(int i)
    {
        bool should = i == floorNumber;

        if (unlocked != should && colliding) audioSource.PlaySFX(openCloseSFX);

        unlocked = should;
        foreach (LerpOnCommand loc in doors)
        {
            if (unlocked && colliding) { loc.SetDestination(1); continue; }
            loc.SetDestination(0);
        }
    }
    public void Lock()
    {
        if (!unlocked) return;
        audioSource.PlaySFX(openCloseSFX);
        unlocked = false;
        foreach (LerpOnCommand loc in doors)
        {
            loc.SetDestination(0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag))
        {
            colliding = true;
            if (!unlocked) return;
            audioSource.PlaySFX(openCloseSFX);
            foreach (LerpOnCommand loc in doors)
            {
                loc.SetDestination(1);
            }

        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag))
        {
            colliding = false;
            if (unlocked) audioSource.PlaySFX(openCloseSFX);
            foreach (LerpOnCommand loc in doors)
            {
                loc.SetDestination(0);
            }
        }
    }
}
