using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;  // Array of item prefabs to spawn
    public Transform centerPoint;     // The center of the radius
    public float radius = 5f;         // Radius of the circular area
    [Header("Item Spacing Settings")]
    [Tooltip("Minimum distance between spawned items.")]
    public float minSpawnDistance = 1f;  // Minimum distance between items
    public int itemCount = 5;         // Number of items to spawn

    private List<Vector2> spawnedPositions = new List<Vector2>(); // List to store spawn positions

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        int attempts = 0; // To prevent infinite loops in edge cases

        for (int i = 0; i < itemCount; i++)
        {
            Vector2 spawnPosition;
            bool validPosition;

            do
            {
                // Generate a random angle in radians
                float angle = Random.Range(0f, 2f * Mathf.PI);

                // Calculate position on the radius line
                float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
                float y = centerPoint.position.y + Mathf.Sin(angle) * radius;
                spawnPosition = new Vector2(x, y);

                // Check if the position is valid (not too close to other items)
                validPosition = IsPositionValid(spawnPosition);

                attempts++;

                // Avoid infinite loops if no valid positions are found
                if (attempts > itemCount * 10)
                {
                    Debug.LogWarning("Could not find a valid position for all items.");
                    return;
                }

            } while (!validPosition);

            // Spawn the item at the valid position
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

            // Add the spawn position to the list
            spawnedPositions.Add(spawnPosition);
        }
    }

    bool IsPositionValid(Vector2 newPosition)
    {
        // Check distance to previously spawned items
        foreach (Vector2 position in spawnedPositions)
        {
            if (Vector2.Distance(newPosition, position) < minSpawnDistance)
            {
                return false; // Too close to an already spawned item
            }
        }

        return true; // Valid position
    }
}
