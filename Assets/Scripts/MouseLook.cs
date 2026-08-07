using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public Transform player;
    float xRotation;
    float yRotation;
    public float yaw;
    public Vector3 eyeOffset = new Vector3(0, 0.7f, -0.2f);

    float bobikTimer;
    Vector3 currentBobikOffset;
    public float bobikSpeed;
    public float AmplitudeX;
    public float AmplitudeY;
    public float bobikSmoothing;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yaw = yRotation;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        if (Input.GetAxisRaw("Horizontal") > 0.1f || Input.GetAxisRaw("Vertical") > 0.1f)
        {
            bobikTimer += Time.deltaTime * bobikSpeed;

            float bobicX = Mathf.Cos(bobikTimer) * AmplitudeX;
            float bobicY = Mathf.Sin(bobikTimer) * AmplitudeY;

            currentBobikOffset = new Vector3(bobicX, bobicY, 0);
        }
        else
        {
            bobikTimer = 0;
            currentBobikOffset = Vector3.Lerp(currentBobikOffset, Vector3.zero, bobikSmoothing);
        }

        Vector3 finalPosition = player.position + eyeOffset;

        finalPosition += transform.right * currentBobikOffset.x;
        finalPosition += transform.up * currentBobikOffset.y;

        transform.position = finalPosition;
    }
}
