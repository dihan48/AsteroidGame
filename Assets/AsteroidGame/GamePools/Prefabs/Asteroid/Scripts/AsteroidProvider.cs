using System;
using UnityEngine;

public class AsteroidProvider : MonoBehaviour
{
    private BaseAsteroid asteroid;
    private AsteroidParts asteroidParts;

    public Action<AsteroidProvider> onClear;

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
        onClear?.Invoke(this);
    }

    private void NotifyClearProvider(BaseAsteroid _asteroid)
    {
        _asteroid.onExplodWithoutSpawnParts -= NotifyClearProvider;
        _asteroid.onExplod -= NotifyClearProvider;
        onClear?.Invoke(this);
    }
}
