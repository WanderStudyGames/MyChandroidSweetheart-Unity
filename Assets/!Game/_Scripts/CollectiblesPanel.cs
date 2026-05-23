using System.Collections.Generic;
using UnityEngine;

public class CollectiblesPanel : MonoBehaviour
{
    [SerializeField] List<CollectibleObjectUI> list = new();
    void Start()
    {
        Check();
    }
    public void Check()
    {
        foreach (var obj in list)
        {
            obj.SetEnabled(WorldData.WorldFlags.Has(obj.saveString));
        }
    }
    public void Enable(int i)
    {
        list[i].SetEnabled(true);
    }
    public void Disable(int i)
    {
        list[i].SetEnabled(false);
    }
}

[System.Serializable]
public class CollectibleObjectUI
{
    public GameObject thumbnail;
    public bool collected;
    public string saveString;
    public void SetEnabled(bool b)
    {
        collected = b;
        thumbnail.SetActive(b);
    }
}