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
        starship.onChangeGamehPoints += ChangeGamehPoints;
    }

    private void ChangeGamehPoints(int countGP)
    {
        text.text = countGP.ToString();
    }
}
