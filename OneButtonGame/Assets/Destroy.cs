using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    // Specify the tag of the object to check for collision with
    public string targetTag = "TargetObject";

    // This method is called when a collision happens
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the specified tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Destroy the current object
            Destroy(gameObject);
        }
    }
}
