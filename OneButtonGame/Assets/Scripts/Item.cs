using UnityEngine;

public class Item : MonoBehaviour
{
    public RandomItemSpawner spawner; // Reference to the spawner script

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Increment player's score
            ScoreManager.Instance.AddPoint();

            // Notify the player's script to change orbit direction
            OrbitingObjectOneButtonWithTrail playerOrbitScript = collision.GetComponent<OrbitingObjectOneButtonWithTrail>();
            if (playerOrbitScript != null)
            {
                playerOrbitScript.ChangeOrbitDirection(); // Call the direction change method
            }

            // Spawn a new item and destroy this one
            spawner.ItemPickedUp();
            Destroy(gameObject); // Destroy the current item
        }
    }
}
