using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float initialActivationDelay = 30f; // Delay before the shark activates
    public float speedIncreaseRate = 3.5f; // Rate at which the speed increases
    public float maxSpeed = 21f; // Maximum speed the shark can reach

    private float currentMoveSpeed; // Current speed of the shark
    private bool sharkActivated = false; // Flag to track if the shark is activated

    private void Start()
    {
        currentMoveSpeed = 0f; // Initial speed set to 0, shark not moving initially
        Invoke("ActivateShark", initialActivationDelay);
    }

    private void Update()
    {
        if (sharkActivated)
        {
            // Move and rotate the shark towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * currentMoveSpeed * Time.deltaTime;

            // Calculate the rotation towards the player
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust rotation speed if needed
        }
    }

    private void ActivateShark()
    {
        sharkActivated = true;
        currentMoveSpeed = 7f; // Set initial speed after activation

        // Start increasing speed over time
        InvokeRepeating("IncreaseSpeed", 30f, 30f);
    }

    private void IncreaseSpeed()
    {
        if (currentMoveSpeed < maxSpeed)
        {
            currentMoveSpeed += speedIncreaseRate;
        }
    }
}