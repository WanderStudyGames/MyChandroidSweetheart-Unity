using UnityEngine;

public class LiftCollider : MonoBehaviour
{
    public Lift lift;

    private void OnTriggerEnter(Collider other)
    {
        //lift.OnLiftTriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        //lift.OnLiftTriggerExit(other);
    }
}
