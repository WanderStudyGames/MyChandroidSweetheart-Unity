using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using VInspector;
[HasLocalHandles]
public class CurveArranger : MonoBehaviour
{
    [SerializeField, LocalHandle] private Vector3 _start = new(-6f, 0, 0);
    [SerializeField, LocalHandle] private Vector3 _elbow1 = new(-2f, 0, 0);
    [SerializeField, LocalHandle] private Vector3 _elbow2 = new(2f, 0, 0);
    [SerializeField, LocalHandle] private Vector3 _end = new(6f, 0, 0);
    [SerializeField] private bool _placeAtEnds = true; // If true, the first and last objects will be placed at the start and end points of the curve
    [SerializeField] private Transform[] _objectsToArrange;
#if UNITY_EDITOR
    private void OnValidate()
    {
        bool isSelected = Selection.Contains(gameObject);
        if (Application.isPlaying || !isActiveAndEnabled || !gameObject.activeInHierarchy || !isSelected) return;
        Arrange();
    }
    [Button("Arrange Objects")]
    private void Arrange()
    {
        int count = _objectsToArrange.Length;
        if (count == 0)
            return;

        if (_placeAtEnds)
        {
            for (int i = 0; i < count; i++)
            {
                float t = (float)i / (count - 1); // t runs from 0 to 1
                var position = GetPointOnCurve(t);
                _objectsToArrange[i].position = position;
                EditorUtility.SetDirty(_objectsToArrange[i].gameObject); // Mark the object as dirty to ensure changes are saved
            }

        }
        else
        {
            int totalSlots = count + 1; // N+2 slots, but t runs from 1/totalSlots to N/totalSlots (inclusive)
            for (int i = 0; i < count; i++)
            {
                float t = (float)(i + 1) / (totalSlots);
                var position = GetPointOnCurve(t);
                _objectsToArrange[i].position = position;
                EditorUtility.SetDirty(_objectsToArrange[i].gameObject); // Mark the object as dirty to ensure changes are saved
            }
        }
    }
#endif
    public Vector3 GetPointOnCurve(float t)
    {
        //t is a value between 0 and 1 representing the percentage along the curve
        //the new position will be calculated using a cubic Bezier curve formula

        if (_start == null || _elbow1 == null || _elbow2 == null || _end == null)
        {
            Debug.LogError("Curve points are not set properly.");
            return Vector3.zero;
        }
        // get the positions of the points in world space
        Vector3 p0 = transform.position + _start;
        Vector3 p1 = transform.position + _elbow1;
        Vector3 p2 = transform.position + _elbow2;
        Vector3 p3 = transform.position + _end;

        // Calculate the point on the cubic Bezier curve using the formula
        // P(t) = (1-t)^3 * P0 + 3(1-t)^2 * t * P1 + 3(1-t) * t^2 * P2 + t^3 * P3

        float oneMinusT = 1 - t; // (1-t)
        Vector3 finalPosition =
            Mathf.Pow(oneMinusT, 3) * p0 +
            3 * Mathf.Pow(oneMinusT, 2) * t * p1 +
            3 * oneMinusT * Mathf.Pow(t, 2) * p2 +
            Mathf.Pow(t, 3) * p3;
        //return the calculated position in world space
        return finalPosition;
    }
    private void OnDrawGizmosSelected()
    {
        if (_start == null || _elbow1 == null || _elbow2 == null || _end == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _start, transform.position + _elbow1);
        Gizmos.DrawLine(transform.position + _elbow1, transform.position + _elbow2);
        Gizmos.DrawLine(transform.position + _elbow2, transform.position + _end);
    }

}
