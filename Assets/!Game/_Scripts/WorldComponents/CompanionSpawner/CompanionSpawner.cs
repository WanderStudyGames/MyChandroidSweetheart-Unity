using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private ExecuteUnityEvent interactObject;
    [SerializeField] private bool overrideExisting;
    public void Spawn()
    {
        if (!CompanionManager.CompanionGOExists || overrideExisting)
        {
            if (interactObject == null) interactObject = CompanionManager.DefaultInteractObject;
            CompanionManager.Spawn(spawnLocation.position, spawnLocation.rotation);
            CompanionManager.SetInteractObject(interactObject);
        }
    }
}
