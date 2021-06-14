using UnityEngine;

[ExecuteInEditMode]
public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private GameWorld gameWorld;
    private Camera gameCamera;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (gameCamera.aspect >= 1 && gameCamera.orthographicSize != gameWorld.MinWorldSideSize)
        {
            gameCamera.orthographicSize = gameWorld.MinWorldSideSize;
        }
        if (gameCamera.aspect < 1 && gameCamera.orthographicSize != gameWorld.MinWorldSideSize / gameCamera.aspect)
        {
            gameCamera.orthographicSize = gameWorld.MinWorldSideSize / gameCamera.aspect;
        }
    }
}
