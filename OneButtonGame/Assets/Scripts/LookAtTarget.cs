using UnityEngine;

public class LookAtTarget2D : MonoBehaviour
{
    // Reference to the target object
    public Transform target;

    void Update()
    {
        // Check if the target is assigned
        if (target != null)
        {
            // Get the direction to the target
            Vector3 direction = target.position - transform.position;

            // Calculate the angle to the target in the 2D plane
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the object to look at the target, only around the Z-axis
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
