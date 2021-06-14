using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField]
    private int startCountAsteroids = 2;

    [SerializeField]
    private int countAddedToAsteroidSpawn = 1;

    [SerializeField]
    private float spawnAsteroidsDelay = 5f;

    [SerializeField]
    private Starship spaceship;

    [SerializeField]
    private ObjectPool asteroidPool;

    [SerializeField]
    private ObjectPool bulletPool;

    [SerializeField]
    private UfoProvider UfoProvider;

    private int countAsteroids;

    private bool IsPause = false;

    private IEnumerator coroutineRespawnAsteroids;

    private void Start()
    {
        countAsteroids = startCountAsteroids;
        SpawnAsteroids();
        asteroidPool.onInUseEmpty += StartRespawnAsteroids;
        spaceship.onExploded += Restart;
        UfoProvider.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < countAsteroids; i++)
        {
            asteroidPool.Get();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        IsPause = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        IsPause = false;
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

    public void Restart()
    {
        if(coroutineRespawnAsteroids != null)
        {
            StopCoroutine(coroutineRespawnAsteroids);
        }

        asteroidPool.onInUseEmpty -= StartRespawnAsteroids;
        spaceship.onExploded -= Restart;

        asteroidPool.AllRelease();
        bulletPool.AllRelease();
        
        UfoProvider.StopSpawn();
        UfoProvider.DeleteUfo();

        Start();
    }
}
