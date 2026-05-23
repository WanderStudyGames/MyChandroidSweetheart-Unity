using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDoorTrigger : MonoBehaviour
{
    [Dependency] [SerializeField] private PlayerManager playerManager;
    [Dependency] [SerializeField] private CompanionManager cm;
    [Dependency] [SerializeField] private SpawnerDoor sd;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PlayerManager.Tag) && CompanionManager.CompanionGOExists)
        {
            sd.Open();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(PlayerManager.Tag) && CompanionManager.CompanionGOExists)
        {
            sd.Close();
        }
    }
}
