using UnityEngine;

public class DataWriteComponent : MonoBehaviour
{
    [SerializeField] private string dataEntryName;
    public void Write()
    {
        WorldData.WorldFlags.Add(dataEntryName);
    }
}
