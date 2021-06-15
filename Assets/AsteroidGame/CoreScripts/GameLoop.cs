using System;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [Header("Basic objects")]
    [SerializeField]
    private Starship spaceship;
    [SerializeField]
    private AsteroidPoolProvider asteroidPoolProvider;
    [SerializeField]
    private ObjectPool bulletPool;
    [SerializeField]
    private UfoProvider ufoProvider;

    public Action<bool> onPause;

    public bool IsGameStarted { get; private set; }
    public bool IsPause {
        get => isPause;
        set
        {
            Time.timeScale = value ? 0: 1;
            onPause?.Invoke(value);
            isPause = value;
        }
    }

    private bool isPause;

    private void Start()
    {
        StartMenu();
    }

    private void StartMenu()
    {
        IsGameStarted = false;
        IsPause = true;
    }

    public void Init()
    {
        spaceship.gameObject.SetActive(true);
        spaceship.onExploded += GameOver;
        ufoProvider.Init();
        asteroidPoolProvider.Init();
    }

    public void Pause()
    {
        if (IsGameStarted)
        {
            IsPause = !IsPause;
        }
    }

    public void StartGame()
    {
        IsPause = false;
        if (IsGameStarted)
        {
            Restart();
        }
        else
        {
            IsGameStarted = true;
            Init();
        }
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
        spaceship.ResetPoints();
        spaceship.Respawn();
        spaceship.gameObject.SetActive(false);
        spaceship.onExploded -= GameOver;

        asteroidPoolProvider.StopSpawn();
        asteroidPoolProvider.AllRelease();

        bulletPool.AllRelease();

        ufoProvider.StopSpawn();
        ufoProvider.DeleteUfo();
    }
}
