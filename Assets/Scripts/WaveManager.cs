using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Send
{
    public GameObject unit;
    public int quantity;
    public float timeBetweenUnits;
    public float timeBeforeNextSend;
}

[System.Serializable]
public class Wave
{
    public List<Send> sends;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public List<Wave> waves; // List of waves
    public float spawnRadius = 30f; // Distance from the center to spawn enemies
    public float randomOffset = 10f; // Random offset for spawn positions

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];

            foreach (Send send in currentWave.sends)
            {
                StartCoroutine(SpawnSend(send));
                yield return new WaitForSeconds(send.timeBeforeNextSend);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(10f); // Delay between waves
        }

        isSpawning = false; // Spawning complete
    }

    private IEnumerator SpawnSend(Send send)
    {
        for (int i = 0; i < send.quantity; i++)
        {
            SpawnUnit(send.unit);
            yield return new WaitForSeconds(send.timeBetweenUnits);
        }
    }

    private void SpawnUnit(GameObject unit)
    {
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        spawnPosition += Random.insideUnitCircle * randomOffset;
        Instantiate(unit, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the Scene view to visualize the spawning circle radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
