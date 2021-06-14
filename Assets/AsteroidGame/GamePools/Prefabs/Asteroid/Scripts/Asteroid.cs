using UnityEngine;

public class Asteroid : MonoBehaviour, IObjectPool
{
    [SerializeField]
    protected float minSpeed = 1f;

    [SerializeField]
    protected float maxSpeed = 6f;

    public event OnRelease onRelease;

    private AsteroidProvider mainAsteroidProvider;

    public void Enable()
    {
        mainAsteroidProvider = GetComponent<AsteroidProvider>();

        float speed = Random.Range(minSpeed, maxSpeed);
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 position = GameWorld.instance.RandomAsteroidPosition();

        mainAsteroidProvider.Init(speed, direction, position);
        mainAsteroidProvider.onClear += Explode;

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        mainAsteroidProvider.onClear -= Explode;
        gameObject.SetActive(false);
    }

    public void Explode(AsteroidProvider asteroidProvider)
    {
        onRelease?.Invoke(this);
    }
}
