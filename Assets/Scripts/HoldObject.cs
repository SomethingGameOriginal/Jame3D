using UnityEngine;

public class HoldObject : MonoBehaviour
{
    Camera mainCamera;
    public float holdDistance = 3; // максимальная дистанция захвата
    public Transform holdPoint;
    public float holdForce = 10; // сила притяжения
    public float maxHoldSpeed = 5f;
    public float damping = 5f; // это сопротивление движению предмета

    Rigidbody heldObject;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, holdDistance))
            {
                if (hit.collider.CompareTag("food"))
                {
                    heldObject = hit.rigidbody;
                }
                else if (hit.collider.CompareTag("bug"))
                {
                    BugPickup bug = hit.collider.GetComponent<BugPickup>();
                    bug.Pickup();

                    heldObject = hit.rigidbody;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            heldObject = null;
        }
    }
    void FixedUpdate()
    {
        if (heldObject != null)
        {
            Vector3 direction = holdPoint.position - heldObject.position;

            heldObject.AddForce(direction * holdForce, ForceMode.Acceleration);
            heldObject.AddForce(-heldObject.linearVelocity * damping, ForceMode.Acceleration);

            heldObject.linearVelocity = Vector3.ClampMagnitude(heldObject.linearVelocity, maxHoldSpeed);
        }
    }
}
