using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float gravity = 1.0f; // strength of gravity
    public float jumpHeight = 5.0f;

    private CharacterController controller; // reference to CharacterController component
    private Vector3 moveDirection = Vector3.zero; // direction of player movement



    // Start is called before the first frame update
    void Start()
    {
        // get reference to CharacterController component
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Jump();
    }

    public void Jump()
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
}
