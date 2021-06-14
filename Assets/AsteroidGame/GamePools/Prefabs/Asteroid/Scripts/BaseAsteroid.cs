using UnityEngine;

public class BaseAsteroid : MonoBehaviour
{
    public delegate void OnExplod(BaseAsteroid asteroid);
    public event OnExplod onExplod;
    public event OnExplod onExplodWithoutSpawnParts;

    public Vector2 Direction
    {
        get
        {
            return direction;
        }
    }

    protected Vector2 direction;
    protected float speed;

    public void Init(float _speed, Vector2 _direction, Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        direction = _direction;
        speed = _speed;
    }

    protected virtual void Update()
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
            onExplod?.Invoke(this);
        }

        IShooter shooter = collider.gameObject.GetComponent<IShooter>();
        if (shooter != null)
        {
            gameObject.SetActive(false);
            onExplodWithoutSpawnParts?.Invoke(this);
        }
    }
}
