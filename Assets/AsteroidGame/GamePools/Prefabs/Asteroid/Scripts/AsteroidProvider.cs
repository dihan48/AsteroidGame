using System;
using System.Collections;
using UnityEngine;

public class AsteroidProvider : MonoBehaviour
{
    public event Action<AsteroidProvider> OnClear;

    private BaseAsteroid asteroid;
    private AsteroidParts asteroidParts;

    private IEnumerator coroutineNotifyClearProviderDelay;

    public void Init(float speed, Vector2 direction, Vector3 position)
    {
        asteroidParts = this.ExtGetComponentInChild<AsteroidParts>();

        if (asteroidParts != null)
        {
            asteroidParts.gameObject.SetActive(false);
            asteroidParts.OnAllClear += NotifyClearProvider;
        }

        asteroid = this.ExtGetComponentInChild<BaseAsteroid>();

        if (asteroid != null)
        {
            asteroid.OnExplodWithoutSpawnParts += NotifyClearProvider;

            if (asteroidParts != null)
            {
                asteroid.OnExplod += SpawnChilds;
            }
            else
            {
                asteroid.OnExplod += NotifyClearProvider;
            }

            asteroid.Init(speed, direction, position);
        }

        gameObject.SetActive(true);
    }

    private void SpawnChilds(BaseAsteroid _asteroid)
    {
        _asteroid.OnExplod -= SpawnChilds;
        asteroidParts.Init(_asteroid.Direction, _asteroid.transform.position);
    }

    private void NotifyClearProvider(AsteroidParts _asteroidParts)
    {
        _asteroidParts.OnAllClear -= NotifyClearProvider;
        if (coroutineNotifyClearProviderDelay != null)
        {
            StopCoroutine(coroutineNotifyClearProviderDelay);
        }
        coroutineNotifyClearProviderDelay = NotifyClearProviderDelay();
        StartCoroutine(coroutineNotifyClearProviderDelay);
    }

    private void NotifyClearProvider(BaseAsteroid _asteroid)
    {
        _asteroid.OnExplodWithoutSpawnParts -= NotifyClearProvider;
        _asteroid.OnExplod -= NotifyClearProvider;
        if (coroutineNotifyClearProviderDelay != null)
        {
            StopCoroutine(coroutineNotifyClearProviderDelay);
        }
        coroutineNotifyClearProviderDelay = NotifyClearProviderDelay();
        StartCoroutine(coroutineNotifyClearProviderDelay);
    }

    private IEnumerator NotifyClearProviderDelay()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if(audio == null)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            if (audio.isPlaying)
            {
                yield return new WaitForSeconds(audio.clip.length - audio.time);
            }
        }

        OnClear?.Invoke(this);
        coroutineNotifyClearProviderDelay = null;
    }
}
