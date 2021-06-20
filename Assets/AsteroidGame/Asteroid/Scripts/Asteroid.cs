using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewAsteroid
{
    public class Asteroid : MonoBehaviour, IObjectPool
    {

        [SerializeField]
        protected float minSpeed = 1f;

        [SerializeField]
        protected float maxSpeed = 6f;

        public System.Action<IObjectPool> OnRelease { get; set; }

        public System.Action<int> OnExplodeProvider;

        private AsteroidProvider mainProvider;

        private int countAllProviders;

        public void Enable()
        {
            mainProvider = GetComponent<AsteroidProvider>();

            mainProvider.OnExplodeProvider += ExplodeProvider;
            mainProvider.OnSideEffectsEnded += Release;

            mainProvider.Init();

            float speed = Random.Range(minSpeed, maxSpeed);
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 position = GameWorld.instance.RandomAsteroidPosition();

            mainProvider.Spawn(position, direction, speed);
            countAllProviders = mainProvider.GetCountAllAvailableProviders();
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void ExplodeProvider()
        {
            int countAllAvailableProvider = mainProvider.GetCountAllAvailableProviders();
            int countExplode = countAllProviders - countAllAvailableProvider;
            countAllProviders -= countExplode;

            OnExplodeProvider?.Invoke(countExplode);
        }

        private void Release()
        {
            if (mainProvider.GetCountAllAvailableProviders() == 0)
            {
                OnRelease?.Invoke(this);
            }
        }

        public int GetCountAllAvailableProviders()
        {
            return mainProvider.GetCountAllAvailableProviders();
        }
    }
}
