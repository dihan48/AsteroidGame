using UnityEngine;

public class Asteroid : MonoBehaviour, IObjectPool
{
    [SerializeField]
    protected float minSpeed = 1f;

    [SerializeField]
    protected float maxSpeed = 6f;

    private AsteroidProvider mainAsteroidProvider;

    public event System.Action<IObjectPool> OnRelease;

    public void Enable()
    {
        mainAsteroidProvider = GetComponent<AsteroidProvider>();

        float speed = Random.Range(minSpeed, maxSpeed);
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 position = GameWorld.instance.RandomAsteroidPosition();

        mainAsteroidProvider.Init(speed, direction, position);
        mainAsteroidProvider.OnClear += Explode;

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        mainAsteroidProvider.OnClear -= Explode;
        gameObject.SetActive(false);
    }

    public void Explode(AsteroidProvider asteroidProvider)
    {
        OnRelease?.Invoke(this);
    }
}
