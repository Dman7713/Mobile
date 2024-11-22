using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetOnEnemyTouch : MonoBehaviour
{
    // This method is called when another collider enters the trigger area of this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Reset the scene when touching an object with the "Enemy" tag
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
