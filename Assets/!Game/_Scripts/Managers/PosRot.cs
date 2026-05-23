using UnityEngine;

public struct PosRot
{
    public Vector3 Position;
    public Quaternion Rotation;
    public static PosRot Default => new();
    public PosRot(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
