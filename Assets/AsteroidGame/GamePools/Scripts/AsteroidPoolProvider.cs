using System.Collections;
using UnityEngine;

public class AsteroidPoolProvider : MonoBehaviour
{
    [SerializeField]
    private int startCountAsteroids = 2;
    [SerializeField]
    private int countAddedToAsteroidSpawn = 1;
    [SerializeField]
    private float spawnAsteroidsDelay = 2f;
    [SerializeField]
    private ObjectPool asteroidPool;

    private int countAsteroids;
    private IEnumerator coroutineRespawnAsteroids;

    public void Init()
    {
        countAsteroids = startCountAsteroids;
        SpawnAsteroids();
        asteroidPool.onInUseEmpty += StartRespawnAsteroids;
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < countAsteroids; i++)
        {
            asteroidPool.Get();
        }
    }

    private void StartRespawnAsteroids()
    {
        coroutineRespawnAsteroids = RespawnAsteroids();
        StartCoroutine(coroutineRespawnAsteroids);
    }

    private IEnumerator RespawnAsteroids()
    {
        yield return new WaitForSeconds(spawnAsteroidsDelay);
        countAsteroids += countAddedToAsteroidSpawn;
        SpawnAsteroids();
    }

    public void StopSpawn()
    {
        if (coroutineRespawnAsteroids != null)
        {
            StopCoroutine(coroutineRespawnAsteroids);
        }
        asteroidPool.onInUseEmpty -= StartRespawnAsteroids;
    }

    public void AllRelease()
    {
        asteroidPool.AllRelease();
    }
}
