using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidAI : MonoBehaviour
{
    public GameObject inkBlobPrefab; // Prefab of the ink blob
    public Transform playerTransform; // Transform of the player object
    public Sprite[] squidSprites; // Array of squid sprites for animation
    public float moveSpeed = 5f; // Speed of squid movement
    public float rotationSpeed = 5f; // Speed of rotation
    public float inkShootInterval = 3f; // Interval between ink shots
    public float inkBlobForce = 10f; // Force with which the ink blob is shot
    //public float squidLifetime = 30f; // Time until the squid disappears
    public float moveToNewSpotDelay = 6f; // Delay after shooting before moving to a new spot
    public float spriteChangeInterval = 0.15f; // Interval between sprite changes
    public float spawnDelay = 60f; // Delay the squid form entering the map

    private float nextInkShootTime;
    private float moveStartTime;
    private float lastSpriteChangeTime;
    private int currentSpriteIndex = 0;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;
    private bool hasEntered = false;
    private float startTime;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not assigned! Please assign the Player Transform in the inspector.");
        }

        // Start ink shooting and movement after 60 seconds
        nextInkShootTime = Time.time + spawnDelay + inkShootInterval;
        moveStartTime = Time.time + spawnDelay;
        lastSpriteChangeTime = Time.time + spawnDelay;

        targetPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite
        if (squidSprites.Length > 0)
            spriteRenderer.sprite = squidSprites[currentSpriteIndex];

        startTime = Time.time;
    }

    void Update()
    {
        if (!hasEntered && Time.time - startTime >= spawnDelay)
        {
            // Move squid in once 60 seconds have passed
            targetPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
            moveStartTime = Time.time;
            hasEntered = true;
        }

        if (hasEntered)
        {
            MoveSquid();
            RotateSquid();

            // Change squid sprite if enough time has passed
            if (Time.time - lastSpriteChangeTime >= spriteChangeInterval)
            {
                lastSpriteChangeTime = Time.time;
                ChangeSprite();
            }

            if (Time.time - moveStartTime >= moveToNewSpotDelay)
            {
                // Move to a new spot on the screen after the delay
                targetPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
                moveStartTime = Time.time;
            }

            if (Time.time >= nextInkShootTime)
            {
                ShootInk();
                nextInkShootTime = Time.time + inkShootInterval;
            }

            //if (Time.time >= moveStartTime + squidLifetime)
            //{
            //    Destroy(gameObject);
            //}
        }
    }

    void MoveSquid()
    {
        // Move squid smoothly towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void RotateSquid()
    {
        if (playerTransform != null)
        {
            // Rotate squid smoothly away from the player
            Vector3 targetDirection = (transform.position - playerTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Player Transform reference is null. Squid rotation skipped.");
        }
    }

    void ChangeSprite()
    {
        // Change squid sprite
        currentSpriteIndex = (currentSpriteIndex + 1) % squidSprites.Length;
        spriteRenderer.sprite = squidSprites[currentSpriteIndex];
    }

    void ShootInk()
    {
        // Calculate direction from squid to player
        Vector3 inkDirection = (playerTransform.position - transform.position).normalized;

        // Define angles for spread of ink blobs (adjust as needed)
        float spreadAngle = 15f;
        float angleIncrement = spreadAngle / 2f;

        // Instantiate ink blobs
        for (int i = 0; i < 3; i++)
        {
            // Calculate offset direction for spread
            Vector3 offsetDirection = Quaternion.Euler(0f, 0f, (i - 1) * angleIncrement) * inkDirection;

            // Instantiate the ink blob prefab at the squid's position
            GameObject inkBlob = Instantiate(inkBlobPrefab, transform.position, Quaternion.identity);

            // Apply force to the ink blob in the direction of the player with spread
            Rigidbody rb = inkBlob.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((offsetDirection + inkDirection) * inkBlobForce, ForceMode.Impulse);
            }

            // Destroy the ink blob after 10 seconds
            Destroy(inkBlob, 10f);
        }
    }
}