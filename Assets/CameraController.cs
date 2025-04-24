using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    private float pitch = 0f; // up/down rotation

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate camera around X axis (vertical look)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalLookLimit, verticalLookLimit);

        // Apply rotations
        transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y + mouseX, 0f);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Project forward and right vectors onto the horizontal plane
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 move = (right * moveX + forward * moveZ).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

        transform.position += move * speed * Time.deltaTime;
    }
}

