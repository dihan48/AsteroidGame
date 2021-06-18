using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource fire;
    [SerializeField]
    private AudioSource acceleration;
    [SerializeField]
    private AudioSource blink;
    [SerializeField]
    private AudioSource explode;

    private Starship starship;

    private void Start()
    {
        starship = GetComponent<Starship>();

        starship.onFire += Fire;
        starship.onAcceleration += Acceleration;
        starship.onExplode += Explode;
        starship.onBlinking += StartBlink;
        starship.onBlinked += StopBlink;
    }

    private void Fire()
    {
        if (fire.isPlaying)
        {
            fire.Stop();
        }
        fire.Play();
    }

    private void Acceleration()
    {
        if (acceleration.isPlaying)
        {
            return;
        }
        acceleration.Play();
    }

    private void StartBlink()
    {
        if (blink.isPlaying)
        {
            return;
        }
        blink.Play();
    }

    private void StopBlink()
    {
        if (blink.isPlaying)
        {
            blink.Stop();
        }
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
