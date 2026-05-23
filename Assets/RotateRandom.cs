using System.Collections;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; } = 5f;
    [SerializeField] private float _spreadRange;

    public void RotateRandomY()
    {
        RotateRandomY(new(0, 360));
    }
    public void RotateRangedYTowardsCompanion()
    {
        var toCompanion = CompanionData.Position - transform.position;
        var lookRotation = Quaternion.LookRotation(toCompanion);
        var yEuler = lookRotation.eulerAngles.y;
        var min = yEuler - _spreadRange;
        var max = yEuler + _spreadRange;
        RotateRandomY(new(min, max));
    }
    private void RotateRandomY(Vector2 range)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Rotate());
        IEnumerator Co_Rotate()
        {
            var absoluteEuler = Random.Range(range.x, range.y);
            var absoluteQuaternion = Quaternion.Euler(0, absoluteEuler, 0);
            var startQuaternion = transform.rotation;
            while (Quaternion.Angle(transform.rotation, absoluteQuaternion) > 0.1f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, absoluteQuaternion, Time.deltaTime * Speed);
                yield return null;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Draw spread range in editor
        Gizmos.color = Color.yellow;
        var toCompanion = CompanionData.Position - transform.position;
        var lookRotation = Quaternion.LookRotation(toCompanion);
        var yEuler = lookRotation.eulerAngles.y;
        var min = yEuler - _spreadRange;
        var max = yEuler + _spreadRange;
        var minDir = Quaternion.Euler(0, min, 0) * Vector3.forward;
        var maxDir = Quaternion.Euler(0, max, 0) * Vector3.forward;
        Gizmos.DrawLine(transform.position, transform.position + minDir * 10f);
        Gizmos.DrawLine(transform.position, transform.position + maxDir * 10f);
    }
}
