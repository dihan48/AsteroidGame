using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameLoop gameLoop;
    [SerializeField]
    private Button continueButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        gameLoop.onPause += (bool isPause) => gameObject.SetActive(isPause);
    }

    private void OnEnable()
    {
        continueButton.interactable = gameLoop.IsGameStarted;
    }
}