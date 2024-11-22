using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // The item prefab to spawn
    public Transform circleCenter; // The center of the circle
    public float circleRadius = 5f; // Radius of the circle

    private GameObject currentItem;

    void Start()
    {
        SpawnItem();
    }

    public void SpawnItem()
    {
        // Destroy the previous item if it exists
        if (currentItem != null)
            Destroy(currentItem);

        // Generate a random angle in radians
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate position on the circle
        Vector3 itemPosition = new Vector3(
            circleCenter.position.x + Mathf.Cos(randomAngle) * circleRadius,
            circleCenter.position.y + Mathf.Sin(randomAngle) * circleRadius,
            0f
        );

        // Spawn the new item and assign it to currentItem
        currentItem = Instantiate(itemPrefab, itemPosition, Quaternion.identity);
        currentItem.GetComponent<Item>().spawner = this; // Assign this spawner to the item
    }

    public void ItemPickedUp()
    {
        // Call this function when the item is picked up to spawn a new item
        SpawnItem();
    }
}
