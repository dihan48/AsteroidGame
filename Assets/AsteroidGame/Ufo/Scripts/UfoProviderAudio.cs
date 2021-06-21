using UnityEngine;

public class UfoProviderAudio : MonoBehaviour
{
    [SerializeField]
    private Ufo ufo;

    [SerializeField]
    private AudioSource explode;

    private void Start()
    {
        ufo.OnExplode += Explode;
    }

    private void Explode()
    {
        if (explode.isPlaying)
        {
            explode.Stop();
        }

        explode.Play();
    }
}

