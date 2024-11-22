using UnityEngine;
using UnityEngine.SceneManagement; // Required to reload the scene

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the orbiting object
        if (collision.CompareTag("Player"))
        {
            // Restart the game by reloading the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
