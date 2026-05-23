using UnityEditor;
using UnityEngine;

public class generatekeyboardbuttons : MonoBehaviour
{
    [SerializeField] private UIKeyboardButton _template;
    [SerializeField] private string characters;

    [ExecuteInEditMode]
    [ContextMenu("Create()")]
    public void Create()
    {
        var array = characters.ToCharArray();
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (PrefabUtility.IsPartOfAnyPrefab(_template))
            {
                var go = PrefabUtility.InstantiatePrefab(_template) as UIKeyboardButton;
                go.gameObject.name = array[i].ToString();
            }
            else
            {
                var go = Instantiate(_template);
                go.gameObject.name = array[i].ToString();
            }
        }
    }
}
