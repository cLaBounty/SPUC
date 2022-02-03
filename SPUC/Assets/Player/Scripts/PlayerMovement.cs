using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public float speed = 12f;
	public float gravity = -9.81f * 2;
	public float jumpHeight = 3f;

	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	public bool isSprinting = false;
	public float spritingMultiplier = 1.5f;

	Vector3 velocity;
	bool isGrounded;

    // Update is called once per frame
    void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if(isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

        float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;
		controller.Move(move * speed * Time.deltaTime);

		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}


		// if(Input.GetKey(KeyCode.LeftShift))
		// {
		// 	isSprinting = true;
		// } else
		// {
		// 	isSprinting = false;
		// }

		// if(isSprinting) 
		// {
		// 	speed *= spritingMultiplier;
		// }
		
		// if(Input.GetButtonDown("Crouch"))
		// {
			
		// }
		// if(Input.GetButtonUp("Crouch"))
		// {

		// }


		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
