using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button btnQuit;
    void Start()
    {
        btnQuit.onClick.AddListener(Quit);
    }
    void Update()
    {
        
    }
    void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
