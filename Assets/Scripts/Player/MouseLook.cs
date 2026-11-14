
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 1000;
    GameObject player;
    PlayerController playerController;
    CharacterController cc;
    float xRotation;

    [Header("Плавность перехода ходьба / бег")]
    public float runTransition = 0.2f;

    float runBlend;
    float runBlendVelocity;

    [Header("Camera Tilt - Горизонталь")]
    public float horizontalTiltAngle = 3f;
    public float horizontalTiltSmooth = 0.15f;

    [Header("Camera Tilt - Вертикаль")]
    public float verticalTiltAngle = 3f;
    public float verticalTiltSensitivity = 0.5f;
    public float verticalTiltSmooth = 0.15f;

    float currentHorizontalTilt;
    float currentVerticalTilt;

    float horizontalTiltVelocity;
    float verticalTiltVelocity;

    [Header("Бобики! Тряска камеры при ходьбе")]
    float bobikTimer;
    Vector3 startLocalPosition;

    public float bobikSpeed;
    public float bobikPlusSpeed;

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

        if (player == null)
        {
            Debug.LogError("MouseLook: Player не найден!");
            enabled = false;
            return;
        }

        playerController = player.GetComponent<PlayerController>();
        cc = player.GetComponent<CharacterController>();

        if (playerController == null || cc == null)
        {
            Debug.LogError("MouseLook: Не найден PlayerController или CharacterController!");
            enabled = false;
            return;
        }

        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // MOUSE LOOK
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        player.transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89, 89);


        // CAMERA BOBBING
        bool canBob = playerController.isMoving && cc.isGrounded;

        float targetRunBlend =
            canBob && Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;

        runBlend = Mathf.SmoothDamp(
            runBlend,
            targetRunBlend,
            ref runBlendVelocity,
            runTransition
        );

        if (canBob)
        {
            // Плавно меняем скорость покачивания
            float currentSpeed = Mathf.Lerp(
                bobikSpeed,
                bobikSpeed + bobikPlusSpeed,
                runBlend
            );

            // Плавно меняем амплитуду
            float currentAmplitudeX = Mathf.Lerp(
                AmplitudeX, AmplitudeXRun, runBlend
            );

            float currentAmplitudeY = Mathf.Lerp(
                AmplitudeY, AmplitudeYRun, runBlend
            );

            bobikTimer += Time.deltaTime * currentSpeed;

            float bobikX = Mathf.Cos(bobikTimer) * currentAmplitudeX;
            float bobikY = Mathf.Sin(bobikTimer) * currentAmplitudeY;

            transform.localPosition = startLocalPosition +
                new Vector3(bobikX, bobikY, 0);
        }
        else
        {
            if (!playerController.isMoving)
                bobikTimer = 0;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startLocalPosition,
                Time.deltaTime * bobikSmoothing
            );
        }
    }

    void LateUpdate()
    {
        // ГОРИЗОНТАЛЬНЫЙ НАКЛОН
        float targetHorizontal = 0f;

        if (playerController.isMoving)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            targetHorizontal = -inputX * horizontalTiltAngle;
        }

        currentHorizontalTilt = Mathf.SmoothDamp(
            currentHorizontalTilt,
            targetHorizontal,
            ref horizontalTiltVelocity,
            horizontalTiltSmooth
        );


        // ВЕРТИКАЛЬНЫЙ НАКЛОН
        float targetVertical = 0f;

        if (!cc.isGrounded)
        {
            float velocityY = playerController.GetVerticalVelocity();

            targetVertical = Mathf.Clamp(
                -velocityY * verticalTiltSensitivity,
                -verticalTiltAngle,
                verticalTiltAngle
            );
        }

        currentVerticalTilt = Mathf.SmoothDamp(
            currentVerticalTilt,
            targetVertical,
            ref verticalTiltVelocity,
            verticalTiltSmooth
        );


        // ОБЩИЙ ПОВОРОТ КАМЕРЫ
        transform.localRotation = Quaternion.Euler(
            xRotation + currentVerticalTilt,
            0f,
            currentHorizontalTilt
        );
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
