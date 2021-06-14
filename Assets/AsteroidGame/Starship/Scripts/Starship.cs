using System.Collections;
using UnityEngine;

public class Starship : MonoBehaviour, IShooter
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

    private Rigidbody rb;
    private Collider collider;

    private Quaternion startRotation;
    private Vector3 startPosition;

    private bool canShot = true;
    private int HP;

    public Material GetBulletMaterial()
    {
        return bulletMeterial;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        startRotation = transform.rotation;
        startPosition = transform.position;
        HP = startHealthhPoints;
    }

    private void Update()
    {
        GameWorld.instance.SetInsideScreenPosition(transform);
        InputHandler();
        ApplySpeedLimit();
    }

    private void ApplySpeedLimit()
    {
        if (maxSpeed < rb.velocity.magnitude)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void InputHandler()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKey("d"))
        {
            transform.Rotate(0f, 0f, -180.0f * speedRotate * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Rotate(0f, 0f, 180.0f * speedRotate * Time.deltaTime);
        }
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.up * accelerate);
        }
        if (Input.GetKeyDown("space"))
        {
            if (canShot)
            {
                Shot();
                canShot = false;
                StartCoroutine(ShotDelay());
            }
        }
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
            HP--;
            if (HP == 0)
            {
                onExploded?.Invoke();
                HP = startHealthhPoints;
            }
            else
            {
                collider.enabled = false;
                StartCoroutine(Blink());
            }
            Debug.Log("Старшип взорвался! Илон будет не доволен... :(");
            transform.rotation = startRotation;
            transform.position = startPosition;
            rb.velocity = Vector3.zero;
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