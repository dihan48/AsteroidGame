using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Starship starship;

    private void Awake()
    {
        starship.OnChangeHealth += ChangeHealth;
    }

    private void ChangeHealth(int countHP)
    {
        text.text = countHP.ToString();
    }
}
