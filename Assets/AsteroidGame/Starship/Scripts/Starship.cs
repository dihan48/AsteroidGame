using System;
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
    private float fireDelay = 0.333f;
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

    public Action onFire;
    public Action onExplode;
    public Action onEndedHealthPoints;
    public Action onAcceleration;
    public Action onBlinking;
    public Action onBlinked;
    public Action<int> onChangeHealth;
    public Action<int> onChangeGamehPoints;

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
    private new Collider collider;

    private Quaternion startRotation;
    private Vector3 startPosition;

    private IEnumerator coroutineFireDelay;
    private IEnumerator coroutineBlink;

    private bool canFire = true;

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
        onAcceleration?.Invoke();
    }
    public void Shooting()
    {
        if (canFire)
        {
            Fire();
            canFire = false;
            coroutineFireDelay = FireDelay();
            StartCoroutine(coroutineFireDelay);
        }
    }

    public void LookAt(Vector3 position)
    {
        Vector2 direction = Vector3.Lerp(transform.up, ((Vector2)position - (Vector2)transform.position).normalized, speedRotate * 4 * Time.deltaTime);
        transform.up = direction;
    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    private void Fire()
    {
        Bullet bullet = (Bullet)bulletPool.Get();
        bullet.Shot(this, bulletDeparturePoint.position, (bulletDeparturePoint.position - transform.position).normalized);
        onFire?.Invoke();
    }

    private void OnTriggerEnter(Collider _collider)
    {
        BaseAsteroid asteroid = _collider.gameObject.GetComponent<BaseAsteroid>();
        NewAsteroid.BaseAsteroid newAsteroid = _collider.gameObject.GetComponent<NewAsteroid.BaseAsteroid>();
        Bullet bullet = _collider.gameObject.GetComponent<Bullet>();
        Ufo ufo = _collider.gameObject.GetComponent<Ufo>();

        if (asteroid != null || newAsteroid != null || (bullet != null && bullet.Shooter != (IShooter)this) || ufo != null)
        {
            Debug.Log("Старшип взорвался! Илон будет не доволен... :(");

            onExplode?.Invoke();

            CountHealthPoints--;
            if (CountHealthPoints == 0)
            {
                onEndedHealthPoints?.Invoke();
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
        onBlinking?.Invoke();
        coroutineBlink = Blink();
        StartCoroutine(coroutineBlink);
    }

    public void Respawn()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
        rb.velocity = Vector3.zero;

        if(coroutineFireDelay != null)
        {
            StopCoroutine(coroutineFireDelay);
        }

        canFire = true;
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
        onBlinked?.Invoke();
        collider.enabled = true;
    }
}