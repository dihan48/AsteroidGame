using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameLoop gameLoop;

    private void Awake()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        Application.Quit();
    }
}
