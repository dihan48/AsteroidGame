using System;
using UnityEngine;

namespace NewAsteroid
{
    public class BaseAsteroid : MonoBehaviour, ItriggerOnBullet
    {
        [SerializeField]
        private int gamePoints = 20;

        public event Action<BaseAsteroid> OnExplod;
        public event Action<BaseAsteroid> OnExplodWithoutSpawn;
        public event Action OnSideEffectsEnded;

        public Vector2 Direction => direction;
        public bool HaveSideEffects { get; set; } = false;

        private Vector2 direction;
        private float speed;

        public void Init(float _speed, Vector2 _direction, Vector3 position)
        {
            transform.position = position;
            direction = _direction;
            speed = _speed;
            gameObject.SetActive(true);
        }

        public void SideEffectsEnded()
        {
            OnSideEffectsEnded?.Invoke();
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

                if(HaveSideEffects == false)
                {
                    SideEffectsEnded();
                }
            }

            IShooter shooter = collider.gameObject.GetComponent<IShooter>();
            if (shooter != null)
            {
                gameObject.SetActive(false);
                OnExplodWithoutSpawn?.Invoke(this);

                if (HaveSideEffects == false)
                {
                    SideEffectsEnded();
                }
            }
        }

        public int GetGamePoints()
        {
            return gamePoints;
        }
    }
}