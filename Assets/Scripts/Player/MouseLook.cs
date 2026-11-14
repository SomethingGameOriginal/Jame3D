using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("вращение")]
    public float mouseSensitivity = 1000;
    // Раньше в формулу входил Time.deltaTime. Движение мыши уже "за кадр", поэтому
    // deltaTime тут не нужен. Множитель 1/60 сохраняет прежнее ощущение чувствительности.
    const float sensitivityScale = 1f / 60f;

    GameObject player;
    PlayerController playerController;
    CharacterController cc;
    float xRotation;

    [Header("бег")]
    public float runTransition = 0.2f;
    float runBlend;
    float runBlendVelocity;

    [Header("вращение камеры по горизонтале")]
    public float horizontalTiltAngle = 3;
    public float horizontalTiltSmooth = 0.15f;

    [Header("вращение камеры по вертикале")]
    public float verticalTiltAngle = 3;
    public float verticalTiltSensitivity = 0.5f;
    public float verticalTiltSmooth = 0.15f;

    float currentHorizontalTilt;
    float currentVerticalTilt;

    float horizontalTiltVelocity;
    float verticalTiltVelocity;

    [Header("Бобики! Тряска камеры при ходьбе")]
    public float bobikSpeed;
    public float bobikPlusSpeed;

    public float AmplitudeX = 0.015f;
    public float AmplitudeY = 0.05f;

    public float AmplitudeXRun = 0.017f;
    public float AmplitudeYRun = 0.07f;

    [Tooltip("За сколько секунд тряска плавно набирает силу")]
    public float bobFadeIn = 0.15f;
    [Tooltip("За сколько секунд тряска плавно затухает")]
    public float bobFadeOut = 0.2f;

    float bobikTimer;
    float bobWeight;          // 0..1 — насколько тряска сейчас "включена"
    float bobWeightVelocity;
    Vector3 startLocalPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = GameObject.Find("Player");

        playerController = player.GetComponent<PlayerController>();
        cc = player.GetComponent<CharacterController>();

        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // вращение камеры
        // Пока курсор не захвачен (alt-tab, пауза, клик в окно) — не крутим,
        // иначе накопленный сдвиг мыши даёт резкий рывок.
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * sensitivityScale;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * sensitivityScale;

            player.transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -89, 89);
        }


        // БОБИКИ
        bool canBob = playerController.isMoving && cc.isGrounded;

        float targetRunBlend = canBob && Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        runBlend = Mathf.SmoothDamp(runBlend, targetRunBlend, ref runBlendVelocity, runTransition);

        // Плавно включаем или выключаем тряску — это убирает дёрганье на старте
        float targetBobWeight = canBob ? 1 : 0;
        float fadeTime = canBob ? bobFadeIn : bobFadeOut;
        bobWeight = Mathf.SmoothDamp(bobWeight, targetBobWeight, ref bobWeightVelocity, fadeTime);

        if (canBob || bobWeight > 0.001f)
        {
            float currentSpeed = Mathf.Lerp(bobikSpeed, bobikSpeed + bobikPlusSpeed, runBlend);

            float currentAmplitudeX = Mathf.Lerp(AmplitudeX, AmplitudeXRun, runBlend);
            float currentAmplitudeY = Mathf.Lerp(AmplitudeY, AmplitudeYRun, runBlend);

            bobikTimer += Time.deltaTime * currentSpeed;

            float bobikX = Mathf.Cos(bobikTimer) * currentAmplitudeX;
            float bobikY = Mathf.Sin(bobikTimer) * currentAmplitudeY;

            // смещение умножаем на вес: на старте оно растёт с нуля, при остановке уходит в ноль
            transform.localPosition = startLocalPosition + new Vector3(bobikX, bobikY, 0) * bobWeight;
        }
        else
        {
            // тряска полностью затухла — спокойно сбрасываем всё
            bobWeight = 0;
            bobWeightVelocity = 0;
            bobikTimer = 0;
            transform.localPosition = startLocalPosition;
        }
    }

    void LateUpdate()
    {
        // горизонтальный наклон
        float targetHorizontal = 0;

        if (playerController.isMoving)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            targetHorizontal = -inputX * horizontalTiltAngle;
        }

        currentHorizontalTilt = Mathf.SmoothDamp(currentHorizontalTilt, targetHorizontal, ref horizontalTiltVelocity, horizontalTiltSmooth);


        // вертикальный наклон
        float targetVertical = 0;

        if (!cc.isGrounded)
        {
            float velocityY = playerController.GetVerticalVelocity();

            targetVertical = Mathf.Clamp(-velocityY * verticalTiltSensitivity, -verticalTiltAngle, verticalTiltAngle);
        }

        currentVerticalTilt = Mathf.SmoothDamp(currentVerticalTilt, targetVertical, ref verticalTiltVelocity, verticalTiltSmooth);


        // общий поворот камеры
        // Клампим ИТОГОВЫЙ угол: xRotation (±89) + наклон (±3) мог перевалить за 90°, // ± (+-) прикольно
        // и камера "перекидывалась" через полюс.
        float pitch = Mathf.Clamp(xRotation + currentVerticalTilt, -89, 89);
        transform.localRotation = Quaternion.Euler(pitch, 0, currentHorizontalTilt);
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}