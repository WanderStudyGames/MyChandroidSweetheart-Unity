using UnityEngine;

public class FileDelete : MonoBehaviour
{
    public void DeleteFile()
    {
        SaveSystem.Delete(SaveSystem.SaveFile);
    }
    public void DeleteFile(int i)
    {
        SaveSystem.Delete(i);
    }
}
