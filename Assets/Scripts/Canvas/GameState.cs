using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Button btnContinue;
    public bool isContinue = false;
    void Start()
    {

    }
    void Update()
    {
        btnContinue.onClick.AddListener(ContinueGame);
    }
    void ContinueGame()
    {
        isContinue = true;
    }
}
