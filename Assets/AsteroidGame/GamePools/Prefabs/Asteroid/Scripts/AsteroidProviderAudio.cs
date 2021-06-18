using UnityEngine;

public class AsteroidProviderAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource explode;

    private BaseAsteroid asteroid;

    private void Start()
    {
        asteroid = this.ExtGetComponentInChild<BaseAsteroid>();

        if (asteroid != null)
        {
            asteroid.onExplod += Explode;
            asteroid.onExplodWithoutSpawnParts += Explode;
        }
    }

    private void Explode(BaseAsteroid _asteroid)
    {
        if (explode.isPlaying)
        {
            explode.Stop();
        }

        explode.Play();
    }
}
