using UnityEngine;

[CreateAssetMenu(fileName = "Nav Mesh Agent Profile", menuName = "ScriptableObjects/AI/Nav Mesh Agent Profile")]
public class NMAProfile : ScriptableObject
{
    public float baseOffset;
    public float speed;
    public float angularSpeed;
    public float acceleration;
    public float stoppingDistance;
    public bool autoBraking;
}
