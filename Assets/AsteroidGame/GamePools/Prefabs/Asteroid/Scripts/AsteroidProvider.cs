using System;
using System.Collections;
using UnityEngine;

public class AsteroidProvider : MonoBehaviour
{
    public Action<AsteroidProvider> onClear;

    private BaseAsteroid asteroid;
    private AsteroidParts asteroidParts;

    private IEnumerator coroutineNotifyClearProviderDelay;

    public void Init(float speed, Vector2 direction, Vector3 position)
    {
        asteroidParts = this.ExtGetComponentInChild<AsteroidParts>();

        if (asteroidParts != null)
        {
            asteroidParts.gameObject.SetActive(false);
            asteroidParts.onAllClear += NotifyClearProvider;
        }

        asteroid = this.ExtGetComponentInChild<BaseAsteroid>();

        if (asteroid != null)
        {
            asteroid.onExplodWithoutSpawnParts += NotifyClearProvider;

            if (asteroidParts != null)
            {
                asteroid.onExplod += SpawnChilds;
            }
            else
            {
                asteroid.onExplod += NotifyClearProvider;
            }

            asteroid.Init(speed, direction, position);
        }

        gameObject.SetActive(true);
    }

    private void SpawnChilds(BaseAsteroid _asteroid)
    {
        _asteroid.onExplod -= SpawnChilds;
        asteroidParts.Init(_asteroid.Direction, _asteroid.transform.position);
    }

    private void NotifyClearProvider(AsteroidParts _asteroidParts)
    {
        _asteroidParts.onAllClear -= NotifyClearProvider;
        if (coroutineNotifyClearProviderDelay != null)
        {
            StopCoroutine(coroutineNotifyClearProviderDelay);
        }
        coroutineNotifyClearProviderDelay = NotifyClearProviderDelay();
        StartCoroutine(coroutineNotifyClearProviderDelay);
    }

    private void NotifyClearProvider(BaseAsteroid _asteroid)
    {
        _asteroid.onExplodWithoutSpawnParts -= NotifyClearProvider;
        _asteroid.onExplod -= NotifyClearProvider;
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

        onClear?.Invoke(this);
        coroutineNotifyClearProviderDelay = null;
    }
}
