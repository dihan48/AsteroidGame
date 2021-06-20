using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroidPool;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip beat1;
    [SerializeField]
    private AudioClip beat2;

    private AsteroidPoolProvider asteroidPoolProvider;
    private ObjectPool objectPool;
    private int countAllProviders = 0;
    private int countAllAvailableProviders = 0;
    private IEnumerator AmbientPlay;
    private float ambientDelay;

    private void Start()
    {
        objectPool = asteroidPool.GetComponent<ObjectPool>();
        asteroidPoolProvider = asteroidPool.GetComponent<AsteroidPoolProvider>();

        asteroidPoolProvider.OnSpawn += Play;
    }

    private void Play()
    {
        List<IObjectPool> inUse = objectPool.InUse;
        foreach (IObjectPool item in inUse)
        {
            NewAsteroid.Asteroid asteroid = item as NewAsteroid.Asteroid;
            if(asteroid != null)
            {
                asteroid.OnExplodeProvider += SetCountAvailableProviders;
                countAllProviders += asteroid.GetCountAllAvailableProviders();
            }
        }

        countAllAvailableProviders = countAllProviders;

        ambientDelay = 1.5f;
        audioSource.clip = beat1;

        if (AmbientPlay != null)
        {
            StopCoroutine(AmbientPlay);
        }

        AmbientPlay = Ambient();
        StartCoroutine(AmbientPlay);
    }

    private IEnumerator Ambient()
    {
        yield return new WaitForSeconds(ambientDelay);

        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }

        AmbientPlay = Ambient();
        StartCoroutine(AmbientPlay);
    }

    private void SetCountAvailableProviders(int count)
    {
        countAllAvailableProviders -= count;

        if(countAllAvailableProviders == 0)
        {
            if (AmbientPlay != null)
            {
                StopCoroutine(AmbientPlay);
            }
        }

        ambientDelay = 0.2f + 1.4f * ((float)countAllAvailableProviders / countAllProviders);
        if (ambientDelay < 0.7)
        {
            audioSource.clip = beat2;
        }
    }
}
