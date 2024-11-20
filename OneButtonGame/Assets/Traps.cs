using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // Array of different object types to spawn
    public float spawnInterval = 2f;    // Time interval between spawns
    public float moveSpeed = 5f;        // Speed of the objects

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    void SpawnObject()
    {
        // Randomly choose an object from the array
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        GameObject selectedObjectPrefab = objectPrefabs[randomIndex];

        // Randomly choose a spawn position from the camera's edges
        Vector3 spawnPosition = GetRandomSpawnPosition();
        spawnPosition.z = 0; // Ensure the spawn position is in 2D space

        // Instantiate the object at the chosen position
        GameObject spawnedObject = Instantiate(selectedObjectPrefab, spawnPosition, Quaternion.identity);

        // Apply rotation if spawned from top or bottom
        if (spawnPosition.y == mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y) // Top spawn
        {
            spawnedObject.transform.Rotate(0, 0, 90); // Rotate 90 degrees if spawned from top
        }
        else if (spawnPosition.y == mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).y) // Bottom spawn
        {
            spawnedObject.transform.Rotate(0, 0, 90); // Rotate 90 degrees if spawned from bottom
        }

        // Start moving the object across the screen
        spawnedObject.AddComponent<MoveAcrossScreen>().Initialize(moveSpeed, spawnPosition);
    }

    // Get a random spawn position from the edges of the camera view
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        // Randomly pick a spawn corner (left, right, top, or bottom)
        int spawnSide = Random.Range(0, 4);

        switch (spawnSide)
        {
            case 0: // Left
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(0, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
            case 1: // Right
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
            case 2: // Bottom
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 0, mainCamera.nearClipPlane));
                break;
            case 3: // Top
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1, mainCamera.nearClipPlane));
                break;
        }
        spawnPosition.z = 0; // Set z to 0 for 2D space
        return spawnPosition;
    }
}

public class MoveAcrossScreen : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    // Initialize the movement with speed and starting position
    public void Initialize(float moveSpeed, Vector3 spawnPosition)
    {
        speed = moveSpeed;
        direction = GetDirectionBasedOnSpawn(spawnPosition); // Set direction based on spawn position
    }

    void Update()
    {
        // Move the object in the assigned direction
        transform.position += direction * speed * Time.deltaTime;

        // Destroy the object if it moves out of the camera bounds
        if (!IsObjectVisible())
        {
            Destroy(gameObject);
        }
    }

    // Determine the direction based on where the object spawned
    Vector3 GetDirectionBasedOnSpawn(Vector3 spawnPosition)
    {
        // Left spawn moves to the right
        if (spawnPosition.x == Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x)
        {
            return Vector3.right;
        }
        // Right spawn moves to the left
        else if (spawnPosition.x == Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x)
        {
            return Vector3.left;
        }
        // Bottom spawn moves upwards
        else if (spawnPosition.y == Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y)
        {
            return Vector3.up;
        }
        // Top spawn moves downwards
        else if (spawnPosition.y == Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y)
        {
            return Vector3.down;
        }

        // Default return (should never reach here)
        return Vector3.zero;
    }

    // Check if the object is still visible on screen (within camera bounds)
    bool IsObjectVisible()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }
}
