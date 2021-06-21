using System;
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

    public event Action OnSpawn;

    private int countAsteroids;
    private IEnumerator coroutineRespawnAsteroids;

    public void Init()
    {
        countAsteroids = startCountAsteroids;
        SpawnAsteroids();
        asteroidPool.OnInUseEmpty += StartRespawnAsteroids;
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < countAsteroids; i++)
        {
            asteroidPool.Get();
        }

        OnSpawn?.Invoke();
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
        asteroidPool.OnInUseEmpty -= StartRespawnAsteroids;
    }

    public void AllRelease()
    {
        asteroidPool.AllRelease();
    }
}
