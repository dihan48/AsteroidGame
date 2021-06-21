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

    private GameLoop gameLoop;

    private void Start()
    {
        starship = GetComponent<Starship>();

        starship.OnFire += Fire;
        starship.OnAcceleration += Acceleration;
        starship.OnExplode += Explode;
        starship.OnBlinking += StartBlink;
        starship.OnBlinked += StopBlink;

        gameLoop = GameWorld.instance.GetComponent<GameLoop>();
        if(gameLoop != null)
        {
            gameLoop.OnPause += SetPauseBlink;
        }
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

    private void SetPauseBlink(bool value)
    {
        if (value)
        {
            blink.Pause();
        }
        else
        {
            blink.UnPause();
        }
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
