using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // The tag to check for
    public string targetTag = "Map";

    // Called when the collider of this object collides with another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Map" tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Destroy this object
            Destroy(gameObject);
        }
    }
}
