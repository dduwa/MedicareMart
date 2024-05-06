using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints; // Array to manage multiple spawn points

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 2.0f, 5.0f); // Starts spawning after 2 seconds, repeats every 5 seconds
    }

    void SpawnCustomer()
    {
        if (customerPrefab != null && spawnPoints.Length > 0)
        {
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(customerPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
            Debug.Log("Customer spawned at: " + selectedSpawnPoint.position);
        }
        else
        {
            Debug.LogError("Customer prefab or spawn points not set!");
        }
    }
}
