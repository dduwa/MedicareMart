using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject[] customerPrefabs; // Array to manage multiple customer types
    public Transform[] spawnPoints; // Array to manage multiple spawn points
    public float spawnDelay = 60f; // Time between spawns in seconds

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
       // Invoke("SpawnCustomer", 2.0f); // Initial delay before the first spawn
    }

    public void SpawnCustomer()
    {
        if (customerPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject selectedCustomerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            Instantiate(selectedCustomerPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
            Debug.Log("Customer spawned at: " + selectedSpawnPoint.position);
            Invoke("SpawnCustomer", spawnDelay);  // Continue spawning at defined intervals
        }
        else
        {
            Debug.LogError("Customer prefabs or spawn points not set!");
        }
    }
}
