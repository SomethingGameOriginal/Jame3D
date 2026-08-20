using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject pause;
    GameState gameState;
    bool isPause = false;
    void Start()
    {
        gameState = pause.GetComponent<GameState>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            pause.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPause || gameState.isContinue)
        {
            pause.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPause = false;
            gameState.isContinue = false;
        }
    }
}
