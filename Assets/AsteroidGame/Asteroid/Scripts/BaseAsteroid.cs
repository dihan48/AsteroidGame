using System;
using UnityEngine;

namespace NewAsteroid
{
    public class BaseAsteroid : MonoBehaviour, ItriggerOnBullet
    {
        [SerializeField]
        private int gamePoints = 20;

        public Action<BaseAsteroid> onExplod;
        public Action<BaseAsteroid> onExplodWithoutSpawn;
        public Action onSideEffectsEnded;

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
            onSideEffectsEnded?.Invoke();
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
                onExplod?.Invoke(this);

                if(HaveSideEffects == false)
                {
                    SideEffectsEnded();
                }
            }

            IShooter shooter = collider.gameObject.GetComponent<IShooter>();
            if (shooter != null)
            {
                gameObject.SetActive(false);
                onExplodWithoutSpawn?.Invoke(this);

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