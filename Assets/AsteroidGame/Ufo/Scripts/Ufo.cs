using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour, IShooter
{
    [SerializeField]
    private Transform Starship;
    [SerializeField]
    private ObjectPool bulletPool;
    [SerializeField]
    private Material bulletMeterial;
    [SerializeField]
    private float minShotDelay = 2f;
    [SerializeField]
    private float maxShotDelay = 5f;

    private int spawnBorderIndex;
    private Vector3 direction;
    private float speed;
    private float shotDelay;

    public delegate void OnExplode();
    public event OnExplode onExplode;

    private IEnumerator coroutineShotDelay;

    public void Init()
    {
        speed = (GameWorld.instance.BorderLeft - GameWorld.instance.BorderRight).magnitude * 0.1f;
        spawnBorderIndex = Random.Range(0, 2);
        direction = new Vector2(spawnBorderIndex > 0 ? 1 : -1, 0);
        transform.position = GameWorld.instance.RandomUfoPosition(spawnBorderIndex);

        if(coroutineShotDelay != null)
        {
            StopCoroutine(coroutineShotDelay);
        }
        
        gameObject.SetActive(true);
        StartShotDelay();
    }

    public Material GetBulletMaterial()
    {
        return bulletMeterial;
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
        gameObject.SetActive(false);
        onExplode?.Invoke();
    }

    private void StartShotDelay()
    {
        coroutineShotDelay = ShotDelay();
        float shotDelayRatio = Random.value;
        shotDelay = Mathf.Lerp(minShotDelay, maxShotDelay, shotDelayRatio);
        StartCoroutine(coroutineShotDelay);
    }

    private IEnumerator ShotDelay()
    {
        while (true){
            yield return new WaitForSeconds(shotDelay);
            Bullet bullet = (Bullet)bulletPool.Get();
            bullet.Shot(this, transform.position, (Starship.position - transform.position).normalized);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        BaseAsteroid asteroid = collider.gameObject.GetComponent<BaseAsteroid>();
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        Starship starship = collider.gameObject.GetComponent<Starship>();

        if (asteroid != null || (bullet != null && bullet.Shooter != this) || starship != null)
        {
            Explode();
        }
    }
}
