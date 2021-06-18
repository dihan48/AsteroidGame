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

    public ControlSchemes ControlScheme { get; set; } = ControlSchemes.mouse;

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

        float v = Input.GetAxisRaw("Vertical");

        if (v > 0)
        {
            starship.Acceleration();
        }

        if (Input.GetKeyDown("space"))
        {
            starship.Shooting();
        }

        if (ControlScheme == ControlSchemes.keyboard)
        {
            KeyboardShemeHandler();
        }

        if (ControlScheme == ControlSchemes.mouse)
        {
            MouseShemeHandler();
        }
    }

    private void KeyboardShemeHandler()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
        {
            starship.TurnRight();
        }
        if (h < 0)
        {
            starship.TurnLeft();
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
