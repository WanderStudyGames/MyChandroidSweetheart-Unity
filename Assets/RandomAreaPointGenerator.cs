using UnityEngine;

public class RandomAreaPointGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 _x;
    [SerializeField] private Vector2 _y;
    [SerializeField] private Vector2 _z;

    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(_x.x, _x.y);
        float y = Random.Range(_y.x, _y.y);
        float z = Random.Range(_z.x, _z.y);
        return new Vector3(x, y, z) + transform.position;
    }
    public Vector3 GetFurthestPoint(Vector3 fromPoint)
    {
        Vector3 furthestPoint = Vector3.zero;
        float maxDistance = float.MinValue;
        // Check the 8 corners of the area to find the furthest point
        for (int i = 0; i < 8; i++)
        {
            float x = (i & 1) == 0 ? _x.x : _x.y;
            float y = (i & 2) == 0 ? _y.x : _y.y;
            float z = (i & 4) == 0 ? _z.x : _z.y;
            Vector3 cornerPoint = new Vector3(x, y, z) + transform.position;
            float distance = Vector3.Distance(fromPoint, cornerPoint);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPoint = cornerPoint;
            }
        }
        return furthestPoint;
    }
    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe cube to represent the area
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + new Vector3((_x.x + _x.y) / 2, (_y.x + _y.y) / 2, (_z.x + _z.y) / 2);
        Vector3 size = new(_x.y - _x.x, _y.y - _y.x, _z.y - _z.x);
        Gizmos.DrawCube(center, size);
    }
}
