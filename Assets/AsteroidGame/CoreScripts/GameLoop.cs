using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [Header("Basic objects")]
    [SerializeField]
    private Starship spaceship;
    [SerializeField]
    private ObjectPool asteroidPool;
    [SerializeField]
    private ObjectPool bulletPool;
    [SerializeField]
    private UfoProvider UfoProvider;

    #region TODO перенести логику к пулу астероидов, здесь ей не место
    [Header("Basic properties of asteroids")] 
    [SerializeField]
    private int startCountAsteroids = 2;
    [SerializeField]
    private int countAddedToAsteroidSpawn = 1;
    [SerializeField]
    private float spawnAsteroidsDelay = 5f;

    private int countAsteroids;
    #endregion

    public delegate void OnPause(bool isPause);
    public event OnPause onPause;

    public bool IsGameStarted => isGameStarted;
    public bool IsPause {
        get => isPause;
        set
        {
            Time.timeScale = value ? 0: 1;
            onPause?.Invoke(value);
            isPause = value;
        }
    }

    private bool isGameStarted;
    private bool isPause;

    private IEnumerator coroutineRespawnAsteroids;

    private void Start()
    {
        StartMenu();
    }

    private void StartMenu()
    {
        isGameStarted = false;
        IsPause = true;
    }

    public void Init()
    {
        countAsteroids = startCountAsteroids;
        SpawnAsteroids();
        asteroidPool.onInUseEmpty += StartRespawnAsteroids;
        spaceship.gameObject.SetActive(true);
        spaceship.onExploded += GameOver;
        UfoProvider.Init();
    }

    public void Pause()
    {
        if (isGameStarted)
        {
            IsPause = !IsPause;
        }
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < countAsteroids; i++)
        {
            asteroidPool.Get();
        }
    }

    public void StartGame()
    {
        IsPause = false;
        if (isGameStarted)
        {
            Restart();
        }
        else
        {
            isGameStarted = true;
            Init();
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

    public void Restart()
    {
        GameEnd();
        Init();
    }

    private void GameOver()
    {
        GameEnd();
        StartMenu();
    }

    private void GameEnd()
    {
        if (coroutineRespawnAsteroids != null)
        {
            StopCoroutine(coroutineRespawnAsteroids);
        }

        spaceship.ResetPoints();
        spaceship.Respawn();
        spaceship.gameObject.SetActive(false);

        asteroidPool.onInUseEmpty -= StartRespawnAsteroids;
        spaceship.onExploded -= GameOver;

        asteroidPool.AllRelease();
        bulletPool.AllRelease();

        UfoProvider.StopSpawn();
        UfoProvider.DeleteUfo();
    }
}
