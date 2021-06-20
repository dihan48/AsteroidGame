using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewAsteroid
{
    public class AsteroidProvider : MonoBehaviour
    {
        [SerializeField]
        private float minPartSpeed = 1f;
        [SerializeField]
        private float maxPartSpeed = 6f;
        [SerializeField]
        private float spreadAngle = 90f;

        public int CountAvailableProviders { get; private set; }

        public System.Action OnExplodeProvider;
        public System.Action OnExplodeWithSpawn;
        public System.Action OnExplodeWithoutSpawn;
        public System.Action OnSideEffectsEnded;

        [SerializeField]
        private AsteroidProvider[] providers;
        [SerializeField]
        private BaseAsteroid asteroid;

        private bool isAvailableAsteroid = true;

        public void Init()
        {
            asteroid = this.ExtGetComponentInChild<BaseAsteroid>();
            if (asteroid != null)
            {
                asteroid.gameObject.SetActive(false);
                asteroid.onExplod += Explode;
                asteroid.onExplodWithoutSpawn += ExplodWithoutSpawn;
                asteroid.onSideEffectsEnded += SideEffectsEnded;
            }

            providers = this.ExtGetComponentsInChild<AsteroidProvider>();
            for (int i = 0; i < providers.Length; i++)
            {
                providers[i].Init();
                providers[i].OnExplodeProvider += ExplodeProvider;
                providers[i].OnSideEffectsEnded += SideEffectsEnded;
            }

            CountAvailableProviders = providers.Length;
            isAvailableAsteroid = true;
        }

        public int GetCountAllAvailableProviders()
        {
            int count = isAvailableAsteroid ? 1 : 0;

            for (int i = 0; i < providers.Length; i++)
            {
                count += providers[i].GetCountAllAvailableProviders();
            }

            return count;
        }

        public void Spawn(Vector3 position, Vector2 direction, float speed)
        {
            if (asteroid != null)
            {
                asteroid.Init(speed, direction, position);
            }
        }

        public void WithoutSpawn()
        {
            isAvailableAsteroid = false;

            if (providers != null && providers.Length > 0)
            {
                for (int i = 0; i < providers.Length; i++)
                {
                    providers[i].WithoutSpawn();
                }
            }
        }

        private void Explode(BaseAsteroid asteroid)
        {
            if (providers != null && providers.Length > 0)
            {
                float speed = Random.Range(minPartSpeed, maxPartSpeed);
                float[] partsAngles = PartsAngles(providers.Length);

                for (int i = 0; i < providers.Length; i++)
                {
                    providers[i].Spawn(asteroid.transform.position, asteroid.Direction.Rotate(partsAngles[i]), speed);
                }
            }

            isAvailableAsteroid = false;
            OnExplodeProvider?.Invoke();
        }

        private void ExplodWithoutSpawn(BaseAsteroid asteroid)
        {
            WithoutSpawn();

            OnExplodeProvider?.Invoke();
        }

        private void ExplodeProvider()
        {
            OnExplodeProvider?.Invoke();
        }

        private void SideEffectsEnded()
        {
            OnSideEffectsEnded?.Invoke();
        }

        private float[] PartsAngles(int countParts)
        {
            float startAngle = 360 - spreadAngle / 2;
            float stepAngle = spreadAngle / (countParts - 1);

            float[] angles = new float[countParts];

            if (angles.Length > 0)
            {
                angles[0] = startAngle;
            }

            for (int i = 1; i < angles.Length; i++)
            {
                angles[i] = angles[i - 1] + stepAngle;
            }

            return angles;
        }
    }
}

