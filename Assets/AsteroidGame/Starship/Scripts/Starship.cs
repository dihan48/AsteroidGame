using System.Collections;
using UnityEngine;

public class Starship : MonoBehaviour, IShooter, ItriggerOnBullet
{
    [SerializeField]
    private float accelerate = 5;
    [SerializeField]
    private float maxSpeed = 10;
    [SerializeField]
    private float speedRotate = 1f;
    [SerializeField]
    private Transform bulletDeparturePoint;
    [SerializeField]
    private ObjectPool bulletPool;
    [SerializeField]
    private float shotDelay = 0.333f;
    [SerializeField]
    private Material bulletMeterial;
    [SerializeField]
    private GameObject starshipModel;
    [SerializeField]
    private float intervalBlinking = 0.5f;
    [SerializeField]
    private float timeBlinking = 3f;
    [SerializeField]
    private int startHealthhPoints = 3;

    public delegate void OnExploded();
    public event OnExploded onExploded;

    public delegate void OnChangeHealth(int countHP);
    public event OnChangeHealth onChangeHealth;

    public delegate void OnChangeGamehPoints(int countGP);
    public event OnChangeGamehPoints onChangeGamehPoints;

    private int countGP;
    private int CountGamePoints
    {
        get
        {
            return countGP;
        }
        set
        {
            onChangeGamehPoints?.Invoke(value);
            countGP = value;
        }
    }

    private int countHP;
    private int CountHealthPoints
    {
        get
        {
            return countHP;
        }
        set
        {
            onChangeHealth?.Invoke(value);
            countHP = value;
        }
    }

    private Rigidbody rb;
    private Collider collider;

    private Quaternion startRotation;
    private Vector3 startPosition;

    private IEnumerator coroutineShotDelay;
    private IEnumerator coroutineBlink;

    private bool canShot = true;

    public Material GetBulletMaterial()
    {
        return bulletMeterial;
    }

    public int GetGamePoints()
    {
        return 0;
    }

    public void AddGamePoints(int gamePoints)
    {
        CountGamePoints += gamePoints;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        startRotation = transform.rotation;
        startPosition = transform.position;
        CountHealthPoints = startHealthhPoints;
        CountGamePoints = 0;
    }

    private void Update()
    {
        GameWorld.instance.SetInsideScreenPosition(transform);
        ApplySpeedLimit();
    }

    private void ApplySpeedLimit()
    {
        if (maxSpeed < rb.velocity.magnitude)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void TurnRight()
    {
        transform.Rotate(0f, 0f, -180.0f * speedRotate * Time.deltaTime);
    }
    public void TurnLeft()
    {
        transform.Rotate(0f, 0f, 180.0f * speedRotate * Time.deltaTime);
    }
    public void Acceleration()
    {
        rb.AddForce(transform.up * accelerate);
    }
    public void Shooting()
    {
        if (canShot)
        {
            Shot();
            canShot = false;
            coroutineShotDelay = ShotDelay();
            StartCoroutine(coroutineShotDelay);
        }
    }

    public void LookAt(Vector3 position)
    {
        Vector2 direction = Vector3.Lerp(transform.up, ((Vector2)position - (Vector2)transform.position).normalized, speedRotate * 4 * Time.deltaTime);
        transform.up = direction;
    }

    private IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        canShot = true;
    }

    private void Shot()
    {
        Bullet bullet = (Bullet)bulletPool.Get();
        bullet.Shot(this, bulletDeparturePoint.position, (bulletDeparturePoint.position - transform.position).normalized);
    }

    private void OnTriggerEnter(Collider _collider)
    {
        BaseAsteroid asteroid = _collider.gameObject.GetComponent<BaseAsteroid>();
        Bullet bullet = _collider.gameObject.GetComponent<Bullet>();
        Ufo ufo = _collider.gameObject.GetComponent<Ufo>();

        if (asteroid != null || (bullet != null && bullet.Shooter != this) || ufo != null)
        {
            Debug.Log("Старшип взорвался! Илон будет не доволен... :(");

            CountHealthPoints--;
            if (CountHealthPoints == 0)
            {
                onExploded?.Invoke();
                ResetPoints();
                Respawn();
            }
            else
            {
                Blinking();
                Respawn();
            }
        }
    }

    private void Blinking()
    {
        collider.enabled = false;
        coroutineBlink = Blink();
        StartCoroutine(coroutineBlink);
    }

    public void Respawn()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
        rb.velocity = Vector3.zero;

        if(coroutineShotDelay != null)
        {
            StopCoroutine(coroutineShotDelay);
        }

        canShot = true;
    }

    public void ResetPoints()
    {
        CountHealthPoints = startHealthhPoints;
        CountGamePoints = 0;

        if (coroutineBlink != null)
        {
            StopCoroutine(coroutineBlink);
        }
    }

    private IEnumerator Blink()
    {
        float startTime = Time.time;
        while (Time.time - startTime < timeBlinking)
        {
            starshipModel.SetActive(false);
            yield return new WaitForSeconds(intervalBlinking * 0.5f);
            starshipModel.SetActive(true);
            yield return new WaitForSeconds(intervalBlinking * 0.5f);
        }
        collider.enabled = true;
    }
}