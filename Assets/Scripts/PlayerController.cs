using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 500.0f;
    public float moveSpeed = 2.0f;
    public float jumpForce = 8.0f;
    public float groundDistance = 1.5f;
    public LayerMask groundMask;

    private float verticalRotation = 0f;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rotate player left and right when mouse is moved side to side
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Move camera up and down when mouse is moved up and down
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Use WASD keys to move player
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(horizontalMovement, 0f, verticalMovement);

        //// Jump when space bar is pressed
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void FixedUpdate()
    {
        // Check if player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);
    }
}

