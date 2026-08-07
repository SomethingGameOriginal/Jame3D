using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public MouseLook mouseLook;

    public int speed;
    Vector3 move;

    bool jumpRequested = false;
    bool isGrounded = false;
    public int jumpForce;
    public float fallMultiplier;

    bool inBufferZone = false;
    bool bufferedJump = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        move = new Vector2(moveX, moveZ).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                jumpRequested = true;
            else if (inBufferZone)
                bufferedJump = true;
        }
    }
    void FixedUpdate()
    {
        Quaternion yawRotation = Quaternion.Euler(0, mouseLook.yaw, 0);
        Vector3 localMovement = new Vector3(move.x, 0f, move.y);
        Vector3 globalMovement = yawRotation * localMovement;

        Vector3 targetVelocity = new Vector3(globalMovement.x * speed, rb.linearVelocity.y, globalMovement.z * speed);
        rb.linearVelocity = targetVelocity;

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
        if (rb.linearVelocity.y < 0)
            rb.AddForce(Vector3.down * (fallMultiplier - .5f) * Physics.gravity.magnitude, ForceMode.Acceleration);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (bufferedJump)
            {
                jumpRequested = true;
                bufferedJump = false;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            inBufferZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            inBufferZone = false;
            bufferedJump = false;
        }
    }
}
