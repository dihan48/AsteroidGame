using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewAsteroid
{
    public class BaseAsteroid : MonoBehaviour, ItriggerOnBullet
    {
        [SerializeField]
        private int gamePoints = 20;

        public Action<BaseAsteroid> onExplod;
        public Action<BaseAsteroid> onExplodWithoutSpawn;

        public Vector2 Direction => direction;
        [SerializeField]
        private Vector2 direction;
        [SerializeField]
        private float speed;

        public void Init(float _speed, Vector2 _direction, Vector3 position)
        {
            transform.position = position;
            direction = _direction;
            speed = _speed;
            gameObject.SetActive(true);
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
            }

            IShooter shooter = collider.gameObject.GetComponent<IShooter>();
            if (shooter != null)
            {
                gameObject.SetActive(false);
                onExplodWithoutSpawn?.Invoke(this);
            }
        }

        public int GetGamePoints()
        {
            return gamePoints;
        }
    }
}