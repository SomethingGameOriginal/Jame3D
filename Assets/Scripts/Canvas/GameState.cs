using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public Button btnContinue;
    public bool isContinue = false;

    public Slider sldrSensitivity;
    public MouseLook mouseLook;

    public Button btnQuitInMenu;
    void Start()
    {
        sldrSensitivity.value = PlayerPrefs.GetFloat("MouseSensitivity", mouseLook.mouseSensitivity);

        btnContinue.onClick.AddListener(ContinueGame);
        btnQuitInMenu.onClick.AddListener(QuitInMenu);
    }
    void Update()
    {
        if (sldrSensitivity.value != mouseLook.mouseSensitivity)
            Sensitivity();
    }
    void ContinueGame()
    {
        isContinue = true;
    }
    void Sensitivity()
    {
        mouseLook.mouseSensitivity = sldrSensitivity.value;

        PlayerPrefs.SetFloat("MouseSensitivity", sldrSensitivity.value);
        PlayerPrefs.Save();
    }
    void QuitInMenu()
    {
        SceneManager.LoadScene(0);
    }
}
