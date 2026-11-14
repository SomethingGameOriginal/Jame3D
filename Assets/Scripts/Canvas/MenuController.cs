using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button btnPlay;
    public Button btnQuit;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        btnPlay.onClick.AddListener(Play);
        btnQuit.onClick.AddListener(Quit);
    }
    void Update()
    {
        
    }
    void Play()
    {
        SceneManager.LoadScene(1);
    }
    void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
