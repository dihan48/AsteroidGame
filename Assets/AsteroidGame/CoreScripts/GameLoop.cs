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

    public event Action<bool> OnPause;

    public bool IsGameStarted { get; private set; }
    public bool IsPause {
        get => isPause;
        set
        {
            Time.timeScale = value ? 0: 1;
            OnPause?.Invoke(value);
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
        spaceship.OnEndedHealthPoints += GameOver;
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
        spaceship.DisableBlinking();
        spaceship.Respawn();

        spaceship.gameObject.SetActive(false);
        spaceship.OnEndedHealthPoints -= GameOver;

        asteroidPoolProvider.StopSpawn();
        asteroidPoolProvider.AllRelease();

        bulletPool.AllRelease();

        ufoProvider.StopSpawn();
        ufoProvider.DeleteUfo();
    }
}
