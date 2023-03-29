using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public GameObject palm;

    public float speed = 6.0f; // speed of player movement
    public float gravity = 1.0f; // strength of gravity
    public float smoothTime = 0.5f;
    public float jumpHeight = 5.0f;
    public float mouseSensitivity = 500.0f;
    public float powerUpRespondRate = 15;
    public bool hasPowerup = false;
    
    private float spawnRange = 9.0f;
    private CharacterController controller; // reference to CharacterController component
    private Vector3 moveDirection = Vector3.zero; // direction of player movement
    private Vector3 smoothMoveVelocity = Vector3.zero; // smoothing velocity for moveDirection
    private float verticalRotation = 0f;

    //public PlayerJump playerJump;

    // Start is called before the first frame update
    void Start()
    {
        //playerJump = GetComponent<PlayerJump>();
        // get reference to CharacterController component
        controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        StartCoroutine(SpawnPowerups());
        
    }

    void Update()
    {
        PlayerMovment();
        //playerJump.Jump();
        Jump();
        CameraConroller();
        RotatePlayerMouse();
    }

    private void PlayerMovment()
    {
        // get horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // calculate movement direction based on input relative to player's local space
        Vector3 direction = transform.TransformDirection(new Vector3(horizontal, 0.0f, vertical));

        // normalize direction to prevent faster diagonal movement
        direction = direction.normalized;

        // calculate player movement
        Vector3 targetVelocity = direction * speed;
        moveDirection = Vector3.SmoothDamp(moveDirection, targetVelocity, ref smoothMoveVelocity, smoothTime);
        moveDirection.y -= gravity * Time.deltaTime;

        // move the player using the CharacterController component
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            //Debug.Log("isGrounded");
            moveDirection.y = 0.0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Space");
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
        }

        // apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // move the player using the CharacterController component
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void CameraConroller()
    {
        // Move camera up and down when mouse is moved up and down
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void RotatePlayerMouse()
    {
        // Rotate player left and right when mouse is moved side to side
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerup = true;
            Debug.Log("Collided");

            // Make the PowerUp game object a child of the Palm
            other.transform.SetParent(palm.transform);

            // Move the PowerUp gameobject to the Palm position with an offset
            Vector3 palmPosition = palm.transform.position;
            Vector3 powerUpPosition = palmPosition + palm.transform.forward * 0.5f + palm.transform.up * 0.1f;
            other.transform.position = powerUpPosition;

            // Set the rotation of the powerup to match the Palm
            Quaternion rotation = palm.transform.rotation;
            other.transform.rotation = rotation;

            // Start the coroutine
            StartCoroutine(PowerupCountdownRoutine());
        }
    }


    private IEnumerator PowerupCountdownRoutine()
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(powerUpRespondRate);
        hasPowerup = false;
        // Destroy the child game object of the Palm
        Transform palmChild = Camera.main.transform.GetChild(0).GetChild(2);
        if (palmChild != null)
        {
            Destroy(palmChild.gameObject);
        }

    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 1.27f, spawnPosZ);

        return randomPos;
    }

    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        }
    }

}
