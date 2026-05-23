using UnityEngine;

[CreateAssetMenu(fileName = "SceneProfile", menuName = "ScriptableObjects/Scenes/SceneProfile")]
public class SceneProfile : ScriptableObject
{
    public BGM BGM;
    [SerializeField] public string sceneFileName;
    public string SceneFileName => sceneFileName;
    [SerializeField] private float bottomlessPitHeight = -50f;
    public float BottomlessPitHeight => bottomlessPitHeight;
}
