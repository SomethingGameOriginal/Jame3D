using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    GameObject player;
    PlayerController playerController;
    float xRotation;

    // БОБИКИ! Тряска камеры
    float bobikTimer;
    Vector3 startLocalPosition;
    public float bobikSpeed;
    public float bobikPlusSpeed; // он прибовляет а не заменяет!!!
    public float AmplitudeX = 0.015f;
    public float AmplitudeY = 0.05f;
    public float AmplitudeXRun = 0.017f;
    public float AmplitudeYRun = 0.07f;
    public float bobikSmoothing;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        startLocalPosition = transform.localPosition;
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        player.transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89, 89);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (playerController.isMoving)
        {
            float bobikX = 0;
            float bobikY = 0;
            if (playerController.cc.isGrounded)
            {
                if (playerController.speed > playerController.speed + playerController.speedShift)
                {
                    bobikTimer += Time.deltaTime * (bobikSpeed + bobikPlusSpeed);
                    bobikX = Mathf.Cos(bobikTimer) * AmplitudeXRun;
                    bobikY = Mathf.Sin(bobikTimer) * AmplitudeYRun;
                }
                else
                {
                    bobikTimer += Time.deltaTime * bobikSpeed;
                    bobikX = Mathf.Cos(bobikTimer) * AmplitudeX;
                    bobikY = Mathf.Sin(bobikTimer) * AmplitudeY;
                }
            }
            transform.localPosition = startLocalPosition + new Vector3(bobikX, bobikY, 0);
        }
        else
        {
            bobikTimer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPosition, Time.deltaTime * bobikSmoothing);
        }
    }
}
