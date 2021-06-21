using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour, IShooter, ItriggerOnBullet
{
    [SerializeField]
    private Transform Starship;
    [SerializeField]
    private ObjectPool bulletPool;
    [SerializeField]
    private Material bulletMeterial;
    [SerializeField]
    private float minFireDelay = 2f;
    [SerializeField]
    private float maxFireDelay = 5f;
    [SerializeField]
    private int gamePoints = 200;

    private int spawnBorderIndex;
    private Vector3 direction;
    private float speed;
    private float fireDelay;

    public event System.Action OnFire;
    public event System.Action OnExplode;

    private IEnumerator coroutineFireDelay;

    public void Init()
    {
        speed = (GameWorld.instance.BorderLeft - GameWorld.instance.BorderRight).magnitude * 0.1f;
        spawnBorderIndex = Random.Range(0, 2);
        direction = new Vector2(spawnBorderIndex > 0 ? 1 : -1, 0);
        transform.position = GameWorld.instance.RandomUfoPosition(spawnBorderIndex);

        if(coroutineFireDelay != null)
        {
            StopCoroutine(coroutineFireDelay);
        }
        
        gameObject.SetActive(true);
        StartFireDelay();
    }

    public Material GetBulletMaterial()
    {
        return bulletMeterial;
    }

    public void AddGamePoints(int gamePoints)
    {
    
    }

    public int GetGamePoints()
    {
        return gamePoints;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Vector3 tempPos = transform.position;
        GameWorld.instance.SetInsideScreenPosition(transform);

        if (tempPos != transform.position)
        {
            Explode();
        }
    }

    private void Explode()
    {
        OnExplode?.Invoke();
        gameObject.SetActive(false);
    }

    private void StartFireDelay()
    {
        coroutineFireDelay = FireDelay();
        float fireDelayRatio = Random.value;
        fireDelay = Mathf.Lerp(minFireDelay, maxFireDelay, fireDelayRatio);
        StartCoroutine(coroutineFireDelay);
    }

    private IEnumerator FireDelay()
    {
        while (true){
            yield return new WaitForSeconds(fireDelay);
            OnFire?.Invoke();
            Bullet bullet = (Bullet)bulletPool.Get();
            bullet.Shot(this, transform.position, (Starship.position - transform.position).normalized);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        BaseAsteroid asteroid = collider.gameObject.GetComponent<BaseAsteroid>();
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        Starship starship = collider.gameObject.GetComponent<Starship>();

        if (asteroid != null || (bullet != null && bullet.Shooter != (IShooter)this) || starship != null)
        {
            Explode();
        }
    }
}
