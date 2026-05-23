using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWebLink : MonoBehaviour
{
    public void Open(string s)
    {
        Application.OpenURL(s);
    }
}
