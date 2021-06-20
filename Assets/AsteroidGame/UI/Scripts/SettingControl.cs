using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Text text;

    [SerializeField]
    private string keyboard = "клавиатура";

    [SerializeField]
    private string mouse = "клавиатура + мышь";

    [SerializeField]
    private InputController inputController;

    private void Awake()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if(inputController.ControlScheme == ControlSchemes.keyboard)
        {
            inputController.ControlScheme = ControlSchemes.mouse;
        }
        else
        {
            inputController.ControlScheme = ControlSchemes.keyboard;
        }
        ChangeButtonText();
    }

    private void OnEnable()
    {
        ChangeButtonText();
    }

    private void ChangeButtonText()
    {
        if (inputController.ControlScheme == ControlSchemes.keyboard)
        {
            text.text = keyboard;
        }
        else
        {
            text.text = mouse;
        }
    }
}
