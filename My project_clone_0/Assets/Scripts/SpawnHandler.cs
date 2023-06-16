using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}