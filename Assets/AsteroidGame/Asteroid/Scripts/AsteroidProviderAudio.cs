using System.Collections;
using UnityEngine;

namespace NewAsteroid
{
    public class AsteroidProviderAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource explode;

        private BaseAsteroid asteroid;

        private void Start()
        {
            asteroid = this.ExtGetComponentInChild<BaseAsteroid>();

            if (asteroid != null)
            {
                asteroid.onExplod += Explode;
                asteroid.onExplodWithoutSpawn += Explode;
                asteroid.HaveSideEffects = true;
            }
        }

        private void Explode(BaseAsteroid _asteroid)
        {
            if (explode.isPlaying)
            {
                explode.Stop();
            }

            explode.Play();
            StartCoroutine(Ended());
        }

        private IEnumerator Ended()
        {
            yield return new WaitForSeconds(explode.clip.length);
            asteroid.SideEffectsEnded();
        }
    }
}
