using UnityEngine;

public enum ControlSchemes
{
    keyboard,
    mouse
}

public class InputController : MonoBehaviour
{
    [SerializeField]
    private Starship starship;
    [SerializeField]
    private GameLoop gameLoop;
    [SerializeField]
    private Camera gameCamera;

    private ControlSchemes controlScheme = ControlSchemes.mouse;
    public ControlSchemes ControlScheme
    {
        get => controlScheme;
        set
        {
            controlScheme = value;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameLoop.Pause();
        }
        if (gameLoop.IsPause)
        {
            return;
        }
        if(controlScheme == ControlSchemes.keyboard)
        {
            KeyboardShemeHandler();
        }
        if (controlScheme == ControlSchemes.mouse)
        {
            MouseShemeHandler();
        }
    }

    private void KeyboardShemeHandler()
    {
        if (Input.GetKey("d"))
        {
            starship.TurnRight();
        }
        if (Input.GetKey("a"))
        {
            starship.TurnLeft();
        }
        if (Input.GetKey("w"))
        {
            starship.Acceleration();
        }
        if (Input.GetKeyDown("space"))
        {
            starship.Shooting();
        }
    }

    private void MouseShemeHandler()
    {
        MousePositionHandler();
        if (Input.GetMouseButton(1))
        {
            starship.Acceleration();
        }
        if (Input.GetMouseButtonDown(0))
        {
            starship.Shooting();
        }
    }

    private void MousePositionHandler()
    {
        Vector3 mousePos = Input.mousePosition;
        starship.LookAt(gameCamera.ScreenToWorldPoint(mousePos));
    }
}
