using System;
using UnityEngine;

public class BaseAsteroid : MonoBehaviour, ItriggerOnBullet
{
    [SerializeField]
    private int gamePoints = 20;

    public event Action<BaseAsteroid> OnExplod;
    public event Action<BaseAsteroid> OnExplodWithoutSpawnParts;

    public Vector2 Direction => direction;

    private Vector2 direction;
    private float speed;

    public void Init(float _speed, Vector2 _direction, Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        direction = _direction;
        speed = _speed;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        GameWorld.instance.SetInsideScreenPosition(transform);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            gameObject.SetActive(false);
            OnExplod?.Invoke(this);
        }

        IShooter shooter = collider.gameObject.GetComponent<IShooter>();
        if (shooter != null)
        {
            gameObject.SetActive(false);
            OnExplodWithoutSpawnParts?.Invoke(this);
        }
    }

    public int GetGamePoints()
    {
        return gamePoints;
    }
}
