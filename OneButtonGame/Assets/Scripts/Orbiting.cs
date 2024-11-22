using UnityEngine;

public class OrbitingObjectOneButtonWithTrail : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform centerObject; // The object to orbit around
    public float orbitRadius = 2f; // Default orbit radius
    public float orbitSpeed = 50f; // Default orbit speed
    public float maxBoostSpeed = 200f; // Maximum boosted speed
    public float boostTime = 3f; // Time to reach max speed during hold

    [Header("Rotation Settings")]
    public float rotationMultiplier = 1f; // Multiplies the z-axis rotation speed

    [Header("Trail Settings")]
    public TrailRenderer trailRenderer; // Assign the Trail Renderer here

    [Header("Audio Settings")]
    public AudioSource pickupSound; // Assign an AudioSource with the pickup sound

    private float angle; // Current angle of rotation
    private int direction = 1; // 1 for clockwise, -1 for counterclockwise
    private float currentSpeed; // Current speed of the orbit
    private float boostTimer = 0f; // Timer for how long the button is held

    private bool isHolding = false; // Is the button currently held?
    private float holdTime = 0f; // Tracks how long the button is held
    private float tapThreshold = 0.2f; // Max time for a tap (vs hold)

    void Start()
    {
        currentSpeed = orbitSpeed; // Initialize orbit speed
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        if (pickupSound == null)
        {
            Debug.LogWarning("Pickup sound not assigned!");
        }
    }

    void Update()
    {
        if (centerObject == null)
        {
            Debug.LogWarning("Center object is not assigned!");
            return;
        }

        // Handle input for both mobile and desktop
        if (IsTouchStart()) // On touch or mouse press
        {
            isHolding = true;
            holdTime = 0f; // Reset hold time
        }

        if (IsTouchHeld()) // While holding touch or mouse
        {
            holdTime += Time.deltaTime; // Track how long the button is held

            // Gradual speed boost
            boostTimer += Time.deltaTime;
            float t = Mathf.Clamp01(boostTimer / boostTime);
            currentSpeed = Mathf.Lerp(orbitSpeed, maxBoostSpeed, t);
        }

        if (IsTouchEnd()) // On touch or mouse release
        {
            isHolding = false;
            boostTimer = 0f;
            currentSpeed = orbitSpeed; // Reset to normal speed

            if (holdTime <= tapThreshold) // Short tap: Change orbit direction
            {
                direction *= -1;
            }
        }

        // Increment the angle based on current speed and direction
        angle += direction * currentSpeed * Time.deltaTime;

        // Convert angle to radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the new position
        float x = centerObject.position.x + Mathf.Cos(angleInRadians) * orbitRadius;
        float y = centerObject.position.y + Mathf.Sin(angleInRadians) * orbitRadius;

        // Apply the new position
        transform.position = new Vector2(x, y);

        // Rotate the object on the z-axis based on direction and speed
        float rotation = direction * currentSpeed * rotationMultiplier * Time.deltaTime;
        transform.Rotate(0f, 0f, rotation);

        // Enable or adjust the trail during specific actions
        if (trailRenderer != null)
        {
            trailRenderer.emitting = true; // Ensure the trail is active
        }
    }

    // Change orbit direction when an item is picked up
    public void ChangeOrbitDirection()
    {
        direction *= -1; // Reverse orbit direction
        PlayPickupSound(); // Play the sound when orbit direction changes
    }

    // Play the pickup sound
    private void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            pickupSound.Play();
        }
    }

    // Check if a touch or mouse button press has started
    private bool IsTouchStart()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0);
    }

    // Check if a touch or mouse button is being held
    private bool IsTouchHeld()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetMouseButton(0);
    }

    // Check if a touch or mouse button has ended
    private bool IsTouchEnd()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0);
    }
}
