using UnityEngine;
using UnityEngine.UI;

public class GamePoints : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Starship starship;

    private void Awake()
    {
        starship.onChangeHealth += ChangeHealth;
    }

    private void ChangeHealth(int countHP)
    {
        text.text = countHP.ToString();
    }
}
