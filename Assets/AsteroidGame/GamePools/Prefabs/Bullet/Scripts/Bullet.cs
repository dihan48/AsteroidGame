using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPool
{
    [SerializeField]
    private float speed = 15;

    public System.Action<IObjectPool> OnRelease { get; set; }

    public IShooter Shooter { get; private set; }

    private float maxDistance = 20;
    private float distance = 0;

    public void Enable()
    {
        gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SubscribeRelease(System.Action<IObjectPool> fnc)
    {
        OnRelease += fnc;
    }

    public void UnsubscribeRelease(System.Action<IObjectPool> fnc)
    {
        OnRelease -= fnc;
    }

    public void Shot(IShooter _shooter, Vector3 startPosition, Vector2 _direction)
    {
        Shooter = _shooter;

        Material bulletMaterial = Shooter.GetBulletMaterial();

        if (bulletMaterial != null)
        {
            GetComponent<MeshRenderer>().material = bulletMaterial;
        }

        maxDistance = Mathf.Abs((GameWorld.instance.BorderRight - GameWorld.instance.BorderLeft).magnitude);
        distance = 0;
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _direction);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 translate = Vector3.up * speed * Time.deltaTime;
        transform.Translate(translate);
        GameWorld.instance.SetInsideScreenPosition(transform);
        distance += translate.magnitude;

        if (distance > maxDistance)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
        OnRelease?.Invoke(this);
    }

    private void OnTriggerEnter(Collider collider)
    {
        ItriggerOnBullet triggerOnBullet = collider.gameObject.GetComponent<ItriggerOnBullet>();

        if (triggerOnBullet != null)
        {
            IShooter _shooter = collider.gameObject.GetComponent<IShooter>();

            if (_shooter == null || Shooter != _shooter)
            {
                Destroy();
                Shooter.AddGamePoints(triggerOnBullet.GetGamePoints());
            }
        }
    }
}
