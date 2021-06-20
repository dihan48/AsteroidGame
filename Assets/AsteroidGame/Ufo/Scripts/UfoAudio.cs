using UnityEngine;

public class UfoAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource move;
    [SerializeField]
    private AudioSource fire;

    private Ufo ufo;

    private GameLoop gameLoop;

    private void Start()
    {
        ufo = GetComponent<Ufo>();

        ufo.onFire += Fire;

        gameLoop = GameWorld.instance.GetComponent<GameLoop>();
        if (gameLoop != null)
        {
            gameLoop.onPause += SetPauseMove;
        }
    }

    private void OnEnable()
    {
        move.Play();
    }

    private void SetPauseMove(bool value)
    {
        if (value)
        {
            move.Pause();
        }
        else
        {
            move.UnPause();
        }
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
