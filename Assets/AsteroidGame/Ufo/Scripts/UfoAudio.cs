using UnityEngine;

public class UfoAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource move;
    [SerializeField]
    private AudioSource fire;
    private Ufo ufo;

    private void Start()
    {
        ufo = GetComponent<Ufo>();

        ufo.onFire += Fire;
    }

    private void OnEnable()
    {
        move.Play();
    }

    private void OnDisable()
    {
        move.Stop();
    }

    private void Fire()
    {
        if (fire.isPlaying)
        {
            fire.Stop();
        }
        fire.Play();
    }
}
