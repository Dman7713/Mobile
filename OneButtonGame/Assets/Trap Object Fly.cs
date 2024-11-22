using UnityEngine;

public class FlyingObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // Object to spawn
    public float spawnInterval = 1f; // Time interval between spawns
    public float moveSpeed = 5f; // Speed of the object movement
    public float spawnHeightMin = -5f; // Minimum Y position for spawn from bottom
    public float spawnHeightMax = 5f; // Maximum Y position for spawn from bottom
    private bool spawnFromLeft = true; // Flag to alternate spawn direction

    private void Start()
    {
        // Start spawning objects at intervals
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    private void SpawnObject()
    {
        Vector2 spawnPos = Vector2.zero;

        // Alternate spawn position between left and bottom
        if (spawnFromLeft)
        {
            spawnPos = new Vector2(-10f, Random.Range(spawnHeightMin, spawnHeightMax)); // Spawn from left with random Y
        }
        else
        {
            spawnPos = new Vector2(Random.Range(-5f, 5f), -5f); // Spawn from bottom with random X
        }

        // Instantiate the object at the spawn position
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        // Set the object's movement direction
        Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();

        if (spawnFromLeft)
        {
            rb.velocity = new Vector2(moveSpeed, 0f); // Move right from the left
        }
        else
        {
            rb.velocity = new Vector2(0f, moveSpeed); // Move upwards from the bottom
        }

        // Alternate the spawn direction for the next object
        spawnFromLeft = !spawnFromLeft;
    }
}
