using UnityEngine;

public class BounceableLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody _bounceable;
    [SerializeField] private float _launchMagnitude;
    public void Launch()
    {
        if (_bounceable == null) return;
        _bounceable.gameObject.SetActive(true);
        _bounceable.Sleep();
        _bounceable.transform.position = transform.position;
        _bounceable.WakeUp();
        _bounceable.velocity = transform.forward * _launchMagnitude;
    }

    private void OnDrawGizmosSelected()
    {
        if (_bounceable == null) return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(transform.position, transform.forward * _launchMagnitude + transform.position);
        Gizmos.DrawSphere(transform.forward * _launchMagnitude + transform.position, 0.2f);

        Gizmos.color = Color.green;

        // Parameters
        Vector3 startPosition = transform.position;
        Vector3 launchVelocity = transform.forward * _launchMagnitude;
        float timeStep = 0.1f; // Smaller values give smoother curves
        float maxTime = 5f; // Adjust based on expected flight duration
        float gravity = Physics.gravity.y; // Unity's gravity

        // Draw the trajectory
        Vector3 previousPosition = startPosition;
        for (float t = 0; t < maxTime; t += timeStep)
        {
            // Calculate the next position
            Vector3 nextPosition = startPosition + launchVelocity * t + 0.5f * new Vector3(0, gravity, 0) * t * t;

            // Draw a line segment
            Gizmos.DrawLine(previousPosition, nextPosition);

            // Update the previous position
            previousPosition = nextPosition;

            // Stop if the object hits the ground (y <= 0)
            if (nextPosition.y <= 0) break;
        }
    }
}
