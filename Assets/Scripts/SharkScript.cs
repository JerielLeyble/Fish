using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    public GameObject sharkPrefab; // Reference to the shark prefab
    public Transform player; // Reference to the player's transform
    public float initialSpawnDelay = 30f; // Delay before the first shark spawns

    private void Start()
    {
        StartCoroutine(SpawnSharkAfterDelay(initialSpawnDelay));
    }

    private IEnumerator SpawnSharkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnShark();
    }

    private void SpawnShark()
    {
        // Instantiate the shark prefab
        Instantiate(sharkPrefab, CalculateSpawnPosition(), Quaternion.identity);
    }

    private Vector3 CalculateSpawnPosition()
    {
        // Calculate the spawn position at the bottom of the screen
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), 0, Camera.main.nearClipPlane));
        spawnPosition.z = 0f; // Ensure the shark spawns at the same Z position as the player
        return spawnPosition;
    }
}