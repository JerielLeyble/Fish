using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HarpoonSpawner : MonoBehaviour
{
    public GameObject harpoonPrefab;
    public Transform playerTransform;
    public float harpoonSpeed = 30f;
    public float deviationAmount = 0f;
    public float despawnTime = 4f;
    public float harpspawnTime = 1f;
    public float elapsedTime = 0f;
    public float maxHarpoonSpawnRate = 0.35f;


    void Start()
    {
        // Start spawning harpoons
        InvokeRepeating("SpawnHarpoon", 1f, harpspawnTime); 
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 30f && harpspawnTime > maxHarpoonSpawnRate)
        {
            harpspawnTime = harpspawnTime / 1.5f;
            CancelInvoke("SpawnHarpoon");
            InvokeRepeating("SpawnHarpoon", 0f, harpspawnTime);
            elapsedTime = 0f;
        }
    }

    void SpawnHarpoon()
    {
        
        // Determine off-screen spawn position
        Vector3 spawnPosition = CalculateOffScreenSpawnPosition();

        // Calculate direction towards the player (only X and Y axes)
        Vector3 directionToPlayer = (playerTransform.position - spawnPosition).normalized;
        directionToPlayer.z = 0f; // Ensure no movement along Z-axis

        // Add deviation to the direction
        Vector3 deviation = Random.insideUnitSphere * deviationAmount;
        deviation.z = 0f; // Ensure no deviation along Z-axis
        directionToPlayer += deviation.normalized;

        // Calculate rotation towards the player (only Y-axis rotation)
        Quaternion rotationToPlayer = Quaternion.LookRotation(Vector3.forward, directionToPlayer);

        // Spawn harpoon and shoot towards the player
        GameObject harpoon = Instantiate(harpoonPrefab, spawnPosition, rotationToPlayer);
        Rigidbody harpoonRigidbody = harpoon.GetComponent<Rigidbody>();
        harpoonRigidbody.velocity = directionToPlayer * harpoonSpeed;

        StartCoroutine(DespawnHarpoon(harpoon));
    }

    IEnumerator DespawnHarpoon(GameObject harpoon)
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(harpoon);
    }


            Vector3 CalculateOffScreenSpawnPosition()
    {
        // Calculate off-screen spawn position (example: above the screen)
        Vector3 screenPoint = new Vector3(Random.Range(0f, Screen.width), Screen.height, 0f);
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        spawnPosition.z = 0f; // Adjust z-coordinate if necessary
        return spawnPosition;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collideWith = coll.gameObject;

        if (collideWith.CompareTag("fish"))
        {
            Destroy(collideWith);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

